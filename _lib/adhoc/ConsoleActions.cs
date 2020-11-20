using System;
using Bam.Net.Application.Network;
using Bam.Net.CommandLine;
using Bam.Net.Testing;

namespace Bam.Net.Adhoc
{
    [Serializable]
    public class ConsoleActions : CommandLineTool
    {
        [ConsoleAction("Test NmapScan")]
        public void TestSomething()
        {
            Remote remote = Remote.For("integration");
            Expect.AreEqual(OSNames.Windows, remote.OS);
            Remote chumbucket3 = Remote.For("192.168.0.241");
            Expect.AreEqual(OSNames.Linux, chumbucket3.OS);
        }
    }
}