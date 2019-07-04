using System;
using System.Collections.Generic;
using System.Linq;
using Bam.Net.Data.Repositories;
using Bam.Net.Server.Streaming;
using Bam.Net.Services.DataReplication.Consensus.Data;

namespace Bam.Net.Services.DataReplication.Consensus
{
    /// <summary>
    /// RaftRing provides local state tracking for a raft consensus implementation.  It also manages communication between RaftNodes.
    /// </summary>
    public class RaftRing : Ring<RaftNode>
    {
        public RaftRing()
        {
            ElectionTimeout = RandomNumber.Between(150, 300);
            Server = new RaftServer();
        }
        
        public string HostName { get; set; }
        public int Port { get; set; }
        
        public int ElectionTimeout { get; set; }
        
        public RaftServer Server { get; set; }
        
        public void NotifyLeaderLogEntryCommitted(RaftLogEntry logEntry)
        {
            // find the leader and call ReceiveReplicateValueResponse on it
            GetLeaderNode().LeaderReceiveWriteResponse(new RaftLogEntryWriteResponse(){LogEntry = logEntry});
        }

        public void NotifyFollowersLogEntryCommitted(RaftLogEntry raftLogEntry)
        {
            throw new NotImplementedException();
        }
        
        protected internal RaftNode GetLeaderNode()
        {
            return FirstArcWhere(a => a.GetTypedServiceProvider().NodeType == RaftNodeType.Leader)
                .GetTypedServiceProvider();
        }

        protected internal List<RaftNode> GetFollowers()
        {
            return ArcsWhere(a => a.GetTypedServiceProvider().NodeType == RaftNodeType.Follower)
                .Select(a => a.GetTypedServiceProvider()).ToList();
        }
        
        protected internal override Arc CreateArc()
        {
            return new Arc<RaftNode>();
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