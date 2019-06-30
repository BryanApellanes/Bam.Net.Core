using System;
using Bam.Net.Data.Repositories;
using Bam.Net.Services.DataReplication.Consensus.Data;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class ConsensusRing : Ring<ConsensusServiceNode>
    {
        public void ReportCommittedLogEntry(ReplicateLogEntryRequest replicationRequest, LogEntry logEntry)
        {
            // find the leader and call ReceiveReplicateValueResponse on it
            GetLeader().ReceiveReplicateValueResponse(new ReplicateLogEntryResponse(){LogEntry = logEntry});
        }

        protected internal ConsensusServiceNode GetLeader()
        {
            throw new NotImplementedException();
        }
        
        protected internal override Arc CreateArc()
        {
            return new Arc<ConsensusServiceNode>();
        }

        public override string GetHashString(object value)
        {
            Args.ThrowIfNull(value);
            return CompositeKeyHashProvider.GetStringKeyHash(value, ",",
                CompositeKeyHashProvider.GetCompositeKeyProperties(value.GetType()));
        }

        public override int GetObjectKey(object value)
        {
            return CompositeKeyHashProvider.GetStringKeyHash(value).ToSha256Int();
        }

        protected override Arc FindArcByObjectKey(int key)
        {
            double slotIndex = Math.Floor((double)(key / ArcSize));
            Arc result = null;
            if (slotIndex < Arcs.Length)
            {
                result = Arcs[(int)slotIndex];
            }

            return result;
        }
    }
}