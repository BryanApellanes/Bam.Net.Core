using System;
using Bam.Net.Automation;
using Bam.Net.CoreServices.ApplicationRegistration.Data;
using Bam.Net.Services;
using Bam.Net.Application.Network;

namespace Bam.Net.Application.Verbs
{
    public class CopyContext
    {
        public string LocalPath { get; set; }
        public string RemotePath { get; set; }
        public Remote Remote { get; set; }

        static object _registryLock;
        static ApplicationServiceRegistry _registry;
        
        public bool Execute()
        {
            throw new NotImplementedException();
        }
    }
}