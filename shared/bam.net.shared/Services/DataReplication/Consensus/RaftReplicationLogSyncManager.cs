using System;
using System.Collections.Generic;
using Bam.Net.Data.Repositories;
using Bam.Net.Logging;
using Bam.Net.Services.DataReplication.Consensus.Data;
using Bam.Net.Services.DataReplication.Consensus.Data.Dao.Repository;
using Bam.Net.Services.DataReplication.Data;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftReplicationLogSyncManager : Loggable, IRaftReplicationLogSyncManager, IRaftReader
    {
        public RaftReplicationLogSyncManager(RaftRing raftRing)
        {
            Args.ThrowIfNull(raftRing, "raftRing");
            RaftRing = raftRing;
        }
        
        public RaftRing RaftRing { get; set; }
        
        public event EventHandler RaftLogEntryCommitted;
        
        protected RaftConsensusRepository LocalRepository => RaftRing.LocalRepository;

        public void HandleReplicationLog(RaftReplicationLog log)
        {
            // foreach commit read the value
            foreach (RaftLogEntryCommit commit in log.Entries)
            {
                RaftLogEntry entry = LocalRepository.LoadByCompositeKey<RaftLogEntry>(commit.RaftLogEntryId);
                // if the value is not found locally retrieve it from the ring and commit locally
                if (entry == null)
                {
                    entry = RaftRing.Retrieve(RetrieveOperation.For(entry)).CopyAs<RaftLogEntry>();
                    entry = LocalRepository.Save(entry);
                }
                //     if the value is found locally then set its state to committed
                else if (entry.State != RaftLogEntryState.Committed)
                {
                    entry.State = RaftLogEntryState.Committed;
                    entry = LocalRepository.Save(entry);
                }
                FireEvent(RaftLogEntryCommitted, this, new RaftLogEntryCommittedEventArgs { CommittingNode = RaftRing.LocalNode.Identifier, DataSourceNode = log.SourceNode, LogEntry = entry});
            }
        }

        public T ReadInstance<T>(ulong compositeKey) where T : CompositeKeyAuditRepoData, new()
        {
            return Retrieve(RetrieveOperation.For(typeof(T), compositeKey.ToString(), UniversalIdentifiers.CKey)).CopyAs<T>();
        }

        public object Retrieve(RetrieveOperation retrieveOperation)
        {
            return RaftRing.Retrieve(retrieveOperation);
        }

        public IEnumerable<object> Query(QueryOperation query)
        {
            return RaftRing.Query(query);
        }
    }
}