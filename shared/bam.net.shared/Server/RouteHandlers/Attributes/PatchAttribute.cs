using System;
using Bam.Net.Web;

namespace Bam.Net.Server.PathHandlers.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PatchAttribute: PathAttribute
    {
        public PatchAttribute(string path) : base(path)
        {
            Verb = HttpVerbs.Patch;
        }
    }
}