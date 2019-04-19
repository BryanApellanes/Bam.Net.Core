using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Bam.Net;
using Bam.Net.Application;
using Bam.Net.CommandLine;


namespace Bam.Shell.Pack
{
    public class JobPackageProvider : Bam.Shell.Pack.PackageProvider
    {
        
        public override void Build(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException("Build is not implemented");
        }        
        public override void Pack(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException("Pack is not implemented");
        }        
        public override void Push(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException("Push is not implemented");
        }        
        public override void Pull(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException("Pull is not implemented");
        }        
    }
}