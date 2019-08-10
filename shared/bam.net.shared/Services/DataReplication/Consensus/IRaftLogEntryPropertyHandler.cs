using System;
using System.Reflection;
using Bam.Net.Services.DataReplication.Consensus.Data;

namespace Bam.Net.Services.DataReplication
{
    public interface IRaftLogEntryPropertyHandler
    {
        string EncodeProperty(PropertyInfo propertyInfo, object readFrom);
        void DecodeProperty<T>(RaftLogEntry raftLogEntry, T setOn);
        void DecodeProperty(RaftLogEntry raftLogEntry, Type setOnType, object setOnInstance);
    }
}