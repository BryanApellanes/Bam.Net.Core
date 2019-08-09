using System;
using Bam.Net.CoreServices.ApplicationRegistration.Data;
using Bam.Net.Data.Repositories;
using Bam.Net.Services.DataReplication.Consensus.Data.Dao.Repository;

namespace Bam.Net.Services.DataReplication.Consensus.Data
{
    public class RaftNodeIdentifier : CompositeKeyAuditRepoData
    {
        public const int DefaultPort = 8417;

        public RaftNodeIdentifier()
        {
        }

        public RaftNodeIdentifier(string hostName, int port)
        {
            HostName = hostName;
            Port = port;
        }

        [CompositeKey]
        public string HostName { get; set; }
        
        [CompositeKey]
        public int Port { get; set; }
        
        public static RaftNodeIdentifier ForCurrentProcess(int port = DefaultPort)
        {
            return ForHost(Machine.Current.Name, port);
        }
        
        /// <summary>
        /// Instantiates an uncommitted RaftNodeIdentifier for the specified hostName and port.
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static RaftNodeIdentifier ForHost(string hostName, int port)
        {
            return new RaftNodeIdentifier()
            {
                HostName = hostName,
                Port = port
            };
        }

        public static ulong KeyFor(string hostName, int port)
        {
            return ForHost(hostName, port).CompositeKeyId;
        }

        public static RaftNodeIdentifier FromRepository(RaftConsensusRepository repository, int port = DefaultPort)
        {
            return FromRepository(repository, Machine.Current.Name, port);
        }

        static readonly object _fromRepoLock = new object();
        public static RaftNodeIdentifier FromRepository(RaftConsensusRepository fromRepository, string hostName,
            int port)
        {
            lock (_fromRepoLock)
            {
                return fromRepository.GetOneRaftNodeIdentifierWhere(rni => rni.HostName == hostName && rni.Port == port);
            }
        }
    }
}