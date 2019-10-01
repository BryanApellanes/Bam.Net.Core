/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using Bam.Net.Yaml;
using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace Bam.Net.Automation
{
    public class WorkerConf
    {
        protected static Dictionary<string, Func<string, WorkerConf>> _deserializers;
        protected static Dictionary<string, Action<string, WorkerConf>> _serializers;
        static WorkerConf()
        {
            _deserializers = new Dictionary<string, Func<string, WorkerConf>>();
            _serializers = new Dictionary<string, Action<string, WorkerConf>>();
            
            _deserializers[".yaml"] = (path) => path.SafeReadFile().FromYaml<WorkerConf>();
            _deserializers[".yml"] = _deserializers[".yaml"];
            
            _deserializers[".json"] = (path) => path.SafeReadFile().FromJson<WorkerConf>();
            
            _deserializers[".xml"] = (path) => path.SafeReadFile().FromXml<WorkerConf>();

            _serializers[".yaml"] = (path, conf) => conf.ToYamlFile(path);
            _serializers[".yml"] = _serializers[".yaml"];

            _serializers[".json"] = (path, conf) => conf.ToJsonFile(path);
            _serializers[".xml"] = (path, conf) => conf.ToXmlFile(path);
        }

        public WorkerConf()
        {
            this._properties = new Dictionary<string, string>();
        }

        public WorkerConf(Worker worker)
            : this()
        {
            this.WorkerTypeName = worker.GetType().AssemblyQualifiedName;
            this.Name = worker.Name;
        }

        public WorkerConf(string name, Type workerType)
            : this()
        {
            this.WorkerTypeName = workerType.AssemblyQualifiedName;
            this.Name = name;
        }

        protected internal Type WorkerType
        {
            get;
            set;
        }
        
        string _workerTypeName;
        public string WorkerTypeName
        {
            get
            {
                return _workerTypeName;
            }
            set
            {
                _workerTypeName = value;
                WorkerType = Type.GetType(value, true);
            }
        }

        public int StepNumber { get; set; }

        public string Name { get; set; }

        public Worker CreateWorker(Job job = null)
        {
            if (WorkerType == null)
            {
                throw new InvalidOperationException($"Specified WorkerTypeName ({WorkerTypeName}) was not found");
            }

            Worker result = WorkerType.Construct<Worker>();
            result.Name = this.Name;
            result.StepNumber = this.StepNumber;
            result.TryConfigure(this, ex => Bam.Net.Logging.Log.Error("Exception configuring worker ({0}): {1}", ex, Name, ex.Message));
            if (job != null)
            {
                result.Job = job;
            }
            return result;
        }

        [XmlIgnore]
        [YamlIgnore]
        [JsonIgnore]
        public string LoadedFrom { get; set; }
        
        public static WorkerConf Load(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLowerInvariant();
            if (!_deserializers.ContainsKey(ext))
            {
                ext = ".yaml";
            }

            WorkerConf workerConf = _deserializers[ext](filePath);
            workerConf.LoadedFrom = filePath;
            return workerConf;
        }

        public void SetProperties(Dictionary<string, string> propertiesToSet)
        {
            Properties = propertiesToSet;
        }

        public void AddProperties(Dictionary<string, string> propertiesToAdd)
        {
            propertiesToAdd.Keys.Each(propName =>
            {
                AddProperty(propName, propertiesToAdd[propName]);
            });
        }

        public void SetProperty(string name, string value)
        {
            if (Properties.ContainsKey(name))
            {
                Properties[name] = value;
            }
            
            Properties.AddMissing(name, value);
        }

        public void AddProperty(string name, string value)
        {
            if (_properties.ContainsKey(name))
            {
                throw new InvalidOperationException("Specified property is already set, use 'SetProperty' to change the value");
            }

            _properties.Add(name, value);
        }

        Dictionary<string, string> _properties;
        public Dictionary<string, string> Properties
        {
            get => _properties;
            set => _properties = value;
        }

        public virtual void Save()
        {
            Save($"./{Name}_WorkerConf.json");
        }

        public void Save(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLowerInvariant();
            if (!_serializers.ContainsKey(ext))
            {
                ext = ".yaml";
            }

            _serializers[ext](filePath, this);
        }
    }
}
