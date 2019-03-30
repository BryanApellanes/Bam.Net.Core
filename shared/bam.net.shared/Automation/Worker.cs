/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bam.Net.Configuration;
using Bam.Net.Logging;
using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace Bam.Net.Automation
{
    public abstract class Worker: Loggable, IWorker, IConfigurable
    {
        public Worker()
            : this(System.Guid.NewGuid().ToString())
        { }

        public Worker(string name, JobManagerService svc = null)
        {
            Name = name ?? System.Guid.NewGuid().ToString();
            JobManagerService = svc;
        }

        JobManagerService _jobManagerService;
        object _jobManagerLock = new object();
        public JobManagerService JobManagerService
        {
            get
            {
                return _jobManagerLock.DoubleCheckLock(ref _jobManagerService, () => new JobManagerService());
            }
            set
            {
                _jobManagerService = value;
            }
        }

        public Job Job { get; set; }
        public string Name { get; set; }
        public bool Busy { get; set; }

        public Status Status { get; set; }
        /// <summary>
        /// Used by the job to sort this worker into its proper
        /// place in order relative to other workers
        /// </summary>
        public int StepNumber
        {
            get;
            set;
        }

        public WorkState WorkState(string jobProperty, object jobValue)
        {
            _state[jobProperty] = jobValue;
            return _state;
        }
        
        WorkState _state;
        public WorkState WorkState(WorkState state = null)
        {
            if (state != null)
            {
                CopyState(state);
            }

            return (WorkState)_state;
        }

        /// <summary>
        /// Gets or sets the current WorkState of this Worker
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public WorkState<T> WorkState<T>(WorkState<T> state = null)
        {
            if (state != null)
            {
                CopyState(state);
            }

            return (WorkState<T>)_state;
        }

        object _doLock = new object();
        public WorkState Do(Job job)
        {
            lock (_doLock)
            {
                Busy = true;
                Job = job;
                WorkState nextWorkState = null;
                
                try
                {
                    nextWorkState = Do(job.CurrentWorkState);
                }
                catch (Exception ex)
                {
                    nextWorkState = new WorkState(this, ex);
                }

                Busy = false;
                return nextWorkState;
            }
        }

        public void Configure(WorkerConf conf)
        {
            Type workerType = this.GetType();
            conf.Properties.Keys.Each(key =>
            {
                PropertyInfo property = workerType.GetProperty(key);
                if (property != null && property.CanWrite)
                {
                    property.SetValue(this, conf.Properties[key]);
                }
            });
        }

        public void SaveConf(string path)
        {
            Type workerType = this.GetType();
            PropertyInfo[] properties = workerType.GetProperties()
                .Where(pi => (pi.PropertyType == typeof(string) || pi.PropertyType.IsArray || pi.PropertyType == typeof(object)) && !pi.Name.Equals("Name"))
                .ToArray();
            WorkerConf conf = new WorkerConf(this)
            {
                StepNumber = StepNumber
            };
            properties.Each(prop =>
            {
                if (!prop.HasCustomAttributeOfType<ExcludeAttribute>())
                {
                    object propVal = prop.GetValue(this);
                    if (propVal != null && prop.PropertyType != typeof(string))
                    {
                        propVal = propVal.ToJson();
                    }
                    conf.AddProperty(prop.Name, propVal?.ToString());
                }
            });
            conf.Save(path);
        }

        /// <summary>
        /// Tells the worker to begin working.  This method is used primarily to
        /// test the execution of a worker in isolation.  Use Job.Run() to execute a full job.
        /// </summary>
        /// <returns></returns>
        public WorkState Do()
        {
            return Do(new InitialWorkState(this));
        }

        public WorkState SetPropertiesFromWorkState(WorkState workState = null)
        {
            workState = workState ?? _state;
            Type type = GetType();
            foreach (PropertyInfo property in type.GetProperties())
            {
                string key = $"{Name}.{property.Name}";
                if (workState.JobData.ContainsKey(key))
                {
                    object data = workState.JobData[key];
                    if (property.PropertyType.IsInstanceOfType(data))
                    {
                        property.SetValue(this, workState.JobData[key]);
                    }
                }
            }

            return workState;
        }

        protected abstract WorkState Do(WorkState currentWorkState);

        [XmlIgnore]
        [YamlIgnore]
        [JsonIgnore]
        [Exclude]
        public abstract string[] RequiredProperties { get;  }
       
        public virtual void Configure(IConfigurer configurer)
        {
            configurer.Configure(this);
            this.CheckRequiredProperties();
        }

        public virtual void Configure(object configuration)
        {
            this.CopyProperties(configuration);
            this.CheckRequiredProperties();
        }
        
        private void CopyState(WorkState state)
        {
            if (state == null)
            {
                return;
            }

            if (_state == null)
            {
                _state = state;
                return;
            }

            Dictionary<string, object> properties = _state?.JobData ?? new Dictionary<string, object>();
            state.JobData.Keys.Each(k => properties.Set(k, state.JobData[k]));
            _state.CopyProperties(state);
            _state.CopyEventHandlers(state);
            _state.JobData = properties;
        }
    }
}
