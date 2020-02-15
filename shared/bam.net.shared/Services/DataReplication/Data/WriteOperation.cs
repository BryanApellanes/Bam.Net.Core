using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.Services.DataReplication.Data
{
    [Serializable]
    public abstract class WriteOperation: Operation
    {  
        public OperationIntent Intent { get; set; }

        /// <summary>
        /// The properties to write
        /// </summary>
        public virtual List<DataProperty> Properties { get; set; }

        public override object Execute(IDistributedRepository repo)
        {
            WriteEvent writeEvent = this.CopyAs<WriteEvent>();            
            Commit(repo, writeEvent);
            Any?.Invoke(this, new OperationEventArgs { WriteEvent = writeEvent });
            return writeEvent;
        }

        /// <summary>
        /// Get the InstanceIdentifier for the current WriteOperation.  Throws an InvalidOperationException
        /// if there are properties with different InstanceIdentifier values.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public virtual string GetInstanceIdentifier()
        {
            EnsurePropertiesExist();

            string instanceId = Properties[0].InstanceIdentifier;
            DataProperty invalid = Properties.FirstOrDefault(p => !p.InstanceIdentifier.Equals(instanceId));
            if (invalid != null)
            {
                throw new InvalidOperationException($"DataProperty.InstanceIdentifier mismatch found: {instanceId} != {invalid.InstanceIdentifier}");
            }
            Args.ThrowIfNullOrEmpty(instanceId, "InstanceIdentifier");
            return instanceId;
        }

        /// <summary>
        /// Get the TypeNamespace for the current WriteOperation.  Throws an InvalidOperationException
        /// if there are properties with different TypeNamespace values.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public string GetTypeNamespace()
        {
            EnsurePropertiesExist();

            string typeNamespace = Properties[0].TypeNamespace;
            DataProperty invalid = Properties.FirstOrDefault(p => !p.TypeNamespace.Equals(typeNamespace));
            if (invalid != null)
            {
                throw new InvalidOperationException($"DataProperty.TypeNamespace mismatch found: {typeNamespace} != {invalid.TypeNamespace}");
            }
            Args.ThrowIfNullOrEmpty(typeNamespace, "TypeNamespace");
            return typeNamespace;
        }

        public string GetTypeName()
        {
            EnsurePropertiesExist();

            string typeName = Properties[0].TypeName;
            DataProperty invalid = Properties.FirstOrDefault(p => !p.TypeName.Equals(typeName));
            if (invalid != null)
            {
                throw new InvalidOperationException($"DataProperty.TypeName mismatch found: {typeName} != {invalid.TypeName}");
            }
            Args.ThrowIfNullOrEmpty(typeName, "TypeName");
            return typeName;
        }
        
        protected void Commit(IDistributedRepository repo, WriteEvent writeEvent)
        {
            repo.Save(SaveOperation.For(writeEvent));
        }

        public static event EventHandler Any;

        public static DataPoint GetDataPoint(object instance)
        {
            return DataPoint.FromInstance(instance);
        } 
        
        protected static List<DataProperty> GetDataProperties(object instance)
        {
            DataPoint dataPoint = GetDataPoint(instance);
            return dataPoint.DataPropertySet.ToList();
        }
        
        private void EnsurePropertiesExist()
        {
            if (Properties == null || Properties.Count == 0)
            {
                throw new InvalidOperationException($"Can't get InstanceIdentifier, no properties specified on {this.GetType().Name}");
            }
        }

    }
}
