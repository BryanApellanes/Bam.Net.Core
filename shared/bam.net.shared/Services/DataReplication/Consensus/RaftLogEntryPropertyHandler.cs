using System;
using System.ComponentModel;
using System.Reflection;
using Bam.Net.Logging;
using Bam.Net.Services.DataReplication.Consensus.Data;

namespace Bam.Net.Services.DataReplication
{
    public class RaftLogEntryPropertyHandler : IRaftLogEntryPropertyHandler
    {
        public RaftLogEntryPropertyHandler()
        {
            Logger = Log.Default;
            TypeMap = new TypeMap();
        }
        
        public TypeMap TypeMap { get; set; }
        public ILogger Logger { get; set; }
        
        public string EncodeProperty(PropertyInfo propertyInfo, object readFrom)
        {
            try
            {
                Args.ThrowIfNull(propertyInfo, "propertyInfo");
                Args.ThrowIfNull(readFrom, "readFrom");

                return propertyInfo.GetValue(readFrom, null).ToBinaryBytes().ToBase64();
            }
            catch (Exception ex)
            {
                Logger.AddEntry("Exception encoding property: {0}", ex, ex.Message);
                return string.Empty;
            }
        }

        public void DecodeProperty<T>(RaftLogEntry raftLogEntry, T setOn)
        {
            try
            {
                Args.ThrowIfNull(raftLogEntry, "raftLogEntry");
                Args.ThrowIfNull(setOn, "setOn");
                
                Type type = typeof(T);
                string propertyName = TypeMap.GetPropertyShortName(raftLogEntry.PropertyId);
                long requestedTypeId = TypeMap.GetTypeId(type);
                Expect.AreEqual(raftLogEntry.TypeId, requestedTypeId, "Types don't match");
                
                PropertyInfo propertyInfo = type.GetProperty(propertyName);
                Args.ThrowIfNull(propertyInfo,
                    $"Property not found for RaftLogEntry ({raftLogEntry.ToString()}): {propertyName}");

                object propertyValue = raftLogEntry.Value.FromBase64().FromBinaryBytes();
                
                propertyInfo.SetValue(setOn, propertyValue);
            }
            catch (Exception ex)
            {
                Logger.AddEntry("Exception decoding property: {0}", ex, ex.Message);
            }
        }
    }
}