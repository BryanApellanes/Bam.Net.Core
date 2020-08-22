using System;
using Bam.Net;
using Bam.Net.Testing;

namespace adhoc
{
    [Serializable]
    class Program : CommandLineTool
    {
        static void Main(string[] args)
        {
            Initialize(args);
        }
    }
}
