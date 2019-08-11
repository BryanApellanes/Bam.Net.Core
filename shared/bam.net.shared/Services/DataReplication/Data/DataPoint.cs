using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.CoreServices.ApplicationRegistration.Data;
using Bam.Net.Data.Repositories;

namespace Bam.Net.Services.DataReplication.Data
{
    /// <summary>
    /// Represents a point of data.  May have multiple data properties.
    /// </summary>
    /// <seealso cref="Bam.Net.Data.Repositories.AuditRepoData" />
    [Serializable]
    public class DataPoint: AuditRepoData
    {
        public DataPoint()
        {
            CreatedBy = Machine.Current.Name;
            DataPropertySet = new DataPropertySet();
        }

        public DataPoint(object instance): this()
        {
            Type instanceType = instance.GetType();
            TypeNamespace = instanceType?.Namespace;
            TypeName = instanceType?.Name;
            AssemblyPath = instanceType?.Assembly?.GetFilePath();
        }
        
        public ulong DataPointOriginId { get; set; }
        public virtual DataPointOrigin DataPointOrigin { get; set; }
        
        public string TypeNamespace { get; set; }
        public string TypeName { get; set; }
        public string AssemblyPath { get; set; }
        public string Description { get; set; }
        public string InstanceIdentifier { get; set; }
        public string DataProperties
        {
            get => DataPropertySet.ToBinaryBytes().ToBase64();
            set => DataPropertySet = value.FromBase64().FromBinaryBytes<DataPropertySet>();
        }
        protected internal DataPropertySet DataPropertySet { get; set; }

        public DataProperty Property(string name, object value = null)
        {
            DataPropertySet.Prop(name, value, out DataProperty prop);
            return prop;
        }

        public T Property<T>(string name)
        {
            return (T)Property(name, null).Value;
        }

        public static DataPoint FromInstance(object instance)
        {
            Args.ThrowIfNull(instance, "instance");
            Type instanceType = instance.GetType();
            return new DataPoint { InstanceIdentifier = RepoData.GetInstanceId(instance)?.ToString(), TypeNamespace = instanceType?.Namespace, TypeName = instanceType?.Name, AssemblyPath = instanceType?.Assembly?.GetFilePath(), Description = $"{instanceType.Namespace}.{instanceType.Name}", DataPropertySet = DataPropertySet.FromInstance(instance) };
        }

        public T ToInstance<T>() where T: class, new()
        {
            T result = new T();
            DataPropertySet.Each(dp => result.Property(dp.Name, dp.Value));            
            return result;
        }
    }
}
