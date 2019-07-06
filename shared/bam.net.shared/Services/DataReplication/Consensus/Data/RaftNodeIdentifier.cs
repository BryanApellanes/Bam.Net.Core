using Bam.Net.CoreServices.ApplicationRegistration.Data;
using Bam.Net.Data.Repositories;

namespace Bam.Net.Services.DataReplication.Consensus.Data
{
    public class RaftNodeIdentifier : CompositeKeyAuditRepoData
    {
        public const int DefaultPort = 8417;
        
        [CompositeKey]
        public string HostName { get; set; }
        
        [CompositeKey]
        public int Port { get; set; }

        public static RaftNodeIdentifier ForCurrentProcess(int port = DefaultPort)
        {
            return ForCurrentProcess(Machine.Current.Name, port);
        }
        
        public static RaftNodeIdentifier ForCurrentProcess(string hostName, int port)
        {
            return new RaftNodeIdentifier()
            {
                HostName = hostName,
                Port = port
            };
        }

        public static ulong IdFor(string hostName, int port)
        {
            return new RaftNodeIdentifier() {HostName = hostName, Port = port}.GetId();
        } 
        
        public ulong GetId()
        {
            return GetULongKeyHash();
        }
    }
}