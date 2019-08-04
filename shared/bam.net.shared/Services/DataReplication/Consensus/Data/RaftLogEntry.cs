using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bam.Net.Data.Repositories;
using Bam.Net.Services.DataReplication.Consensus.Data.Dao.Repository;
using Bam.Net.Services.DataReplication.Data;

namespace Bam.Net.Services.DataReplication.Consensus.Data
{
    [Serializable]
    public class RaftLogEntry: CompositeKeyAuditRepoData
    {
        public RaftLogEntryState State { get; set; }

        /// <summary>
        /// The universal identifier for the data value Instance.
        /// </summary>
        [CompositeKey]
        public ulong InstanceId { get; set; }

        [CompositeKey]
        public long TypeId { get; set; }
        
        [CompositeKey]
        public long PropertyId { get; set; }
        
        [CompositeKey]
        public string Value { get; set; }
        
        public ulong GetId()
        {
            return GetULongKeyHash();
        }

        /// <summary>
        /// Ensures that the current RaftLogEntry exists in the specified repository.
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        public RaftLogEntry GetFromRepository(Repository repository)
        {
            return repository.GetByCompositeKey<RaftLogEntry>(this);
        } 
        
        public static IEnumerable<RaftLogEntry> FromInstance(CompositeKeyAuditRepoData instance)
        {
            if (instance is RaftLogEntry raftLogEntry)
            {
                yield return raftLogEntry;
            }
            else
            {
                Args.ThrowIfNull(instance, "instance");
                Type type = instance.GetType();
                instance.Id = instance.GetULongKeyHash();
                long typeId = TypeMap.GetTypeId(type);
                foreach (PropertyInfo prop in type.GetProperties()
                    .Where(p => p.PropertyType.IsValueType || p.PropertyType == typeof(string)).ToArray())
                {
                    yield return new RaftLogEntry()
                    {
                        InstanceId = instance.Id,
                        TypeId = typeId,
                        PropertyId = TypeMap.GetPropertyId(prop),
                        Value = prop.GetValue(instance, null).ToString()
                    };
                }
            }
        }

        public static IEnumerable<RaftLogEntry> FromDataPropertySet(DataPropertySet dataPropertySet)
        {
            foreach (DataProperty dataProperty in dataPropertySet)
            {
                yield return FromDataProperty(dataProperty);
            }
        }

        public static RaftLogEntry FromDataProperty(DataProperty dataProperty)
        {
            throw new NotImplementedException();
        }
    }
}