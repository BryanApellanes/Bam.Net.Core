using System;
using Bam.Net.Services.DataReplication.Consensus.Data;
using Bam.Net.Services.DataReplication.Data;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftRequest
    {
        public RaftRequest()
        {
            CollationAlgorithm = HashAlgorithms.SHA256;
        }
        
        public string OriginHostName { get; set; }
        public int OriginPort { get; set; }
        public RaftRequestType RequestType { get; set; }
        public int ElectionTerm { get; set; }

        public HashAlgorithms CollationAlgorithm { get; private set; }
        
        string _collationPath;

        /// <summary>
        /// A human readable formatted path specifying the originator, type and time of the request.
        /// </summary>
        public string CollationPath
        {
            get
            {
                if (string.IsNullOrEmpty(_collationPath))
                {
                    _collationPath =
                        $"{OriginHostName}:{OriginPort}/{ElectionTerm}/{RequestType.ToString()}/{Instant.Now.ToString()}";
                }

                return _collationPath;
            }
            set => _collationPath = value;
        }

        public ulong CollationId => CollationPath.ToHashULong(CollationAlgorithm);
        
        /// <summary>
        /// For write RequestTypes 
        /// </summary>
        public RaftLogEntryWriteRequest WriteRequest { get; set; }

        /// <summary>
        /// Response to a vote request or null for non vote requests.
        /// </summary>
        public RaftVote VoteResponse { get; set; }
        
        public RaftReplicationLog LogSyncResponse { get; set; }
        
        public Operation Operation { get; set; }
        
        public ulong? CommitSeq { get; set; }
        
        public RaftNodeIdentifier RequesterIdentifier()
        {
            return new RaftNodeIdentifier(OriginHostName, OriginPort);
        }

        public RaftProtocolClient GetResponseClient()
        {
            return new RaftProtocolClient(OriginHostName, OriginPort);
        }
    }
}