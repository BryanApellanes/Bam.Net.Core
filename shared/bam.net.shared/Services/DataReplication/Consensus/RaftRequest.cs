using System;
using Bam.Net.Services.DataReplication.Consensus.Data;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftRequest
    {
        public RaftRequest()
        {
            CollationAlgorithm = HashAlgorithms.SHA256;
        }
        
        public string RequesterHostName { get; set; }
        public int RequesterPort { get; set; }
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
                        $"{RequesterHostName}:{RequesterPort}/{ElectionTerm}/{RequestType.ToString()}/{Instant.Now.ToString()}";
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
        
        public RaftNodeIdentifier RequesterIdentifier()
        {
            return new RaftNodeIdentifier(RequesterHostName, RequesterPort);
        }

        public RaftClient GetResponseClient()
        {
            return new RaftClient(RequesterHostName, RequesterPort);
        }
    }
}