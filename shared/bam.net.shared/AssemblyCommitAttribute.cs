using System;

namespace Bam.Net
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AssemblyCommitAttribute : Attribute
    {
        public AssemblyCommitAttribute(string commit)
        {
            Commit = commit;
        }
        
        public string Commit { get; set; }
    }
}