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
    public class DataPoint: KeyedAuditRepoData
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
        
        [CompositeKey]
        public string TypeNamespace { get; set; }
        
        [CompositeKey]
        public string TypeName { get; set; }
        
        [CompositeKey]
        public string AssemblyPath { get; set; }
        public string Description { get; set; }
        
        [CompositeKey]
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

        public static DataPoint FromSaveOperation(SaveOperation saveOperation)
        {
            Args.ThrowIfNull(saveOperation, "saveOperation");
            
            return new DataPoint{InstanceIdentifier = saveOperation.GetInstanceIdentifier(), TypeNamespace = saveOperation.GetTypeNamespace(), TypeName = saveOperation.GetTypeName(), DataPropertySet = DataPropertySet.FromSaveOperation(saveOperation)};
        }

        public static DataPoint FromWriteOperation(WriteOperation writeOperation)
        {
            Args.ThrowIfNull(writeOperation, "writeOperation");
            return new DataPoint{InstanceIdentifier = writeOperation.GetInstanceIdentifier(), TypeNamespace = writeOperation.GetTypeNamespace(), TypeName = writeOperation.GetTypeName(), DataPropertySet = DataPropertySet.FromWriteOperation(writeOperation)};
        }

        public static DataPoint FromCreateOperation(CreateOperation createOperation)
        {
            Args.ThrowIfNull(createOperation, "createOperation");
            
            return new DataPoint{ TypeNamespace = createOperation.GetTypeNamespace(), TypeName = createOperation.GetTypeName(), DataPropertySet = DataPropertySet.FromCreateOperation(createOperation)};
        }
        
        public T ToInstance<T>() where T: class, new()
        {
            T result = new T();
            DataPropertySet.Each(dp => result.Property(dp.Name, dp.Value));            
            return result;
        }
    }
}
