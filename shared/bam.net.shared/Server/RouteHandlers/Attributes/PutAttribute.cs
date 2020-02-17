using System;
using Bam.Net.Web;

namespace Bam.Net.Server.PathHandlers.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PutAttribute: PathAttribute
    {
        public PutAttribute(string path) : base(path)
        {
            Verb = HttpVerbs.Put;
        }
    }
}