using Bam.Net.CommandLine;
using Bam.Net.CoreServices;
using Bam.Net.Services.Chunking;
using System.Threading;
using Bam.Net.Testing;

namespace Bam.Net.Server
{
    internal class Chunker : CommandLineTestInterface
    {
        static void Main(string[] args)
        {
            Server.Start();
            Pause("ChunkServer started");
        }

        static ChunkServer _server;
        static object _serverLock = new object();
        public static ChunkServer Server
        {
            get
            {
                return _serverLock.DoubleCheckLock(ref _server, () => new ChunkServer(new FileSystemChunkStorage()));
            }
        }

    }
}
