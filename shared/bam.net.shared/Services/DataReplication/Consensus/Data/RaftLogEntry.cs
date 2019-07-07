using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bam.Net.Data.Repositories;

namespace Bam.Net.Services.DataReplication.Consensus.Data
{
    [Serializable]
    public class RaftLogEntry: CompositeKeyAuditRepoData
    {
        public RaftLogEntryState State { get; set; }

        /// <summary>
        /// The universal identifier for the Instance.
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
    }
}