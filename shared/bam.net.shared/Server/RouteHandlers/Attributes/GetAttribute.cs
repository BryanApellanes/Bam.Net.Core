using System;
using Bam.Net.Web;

namespace Bam.Net.Server.PathHandlers.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class GetAttribute: PathAttribute
    {
        public GetAttribute(string path) : base(path)
        {
            Verb = HttpVerbs.Get;
        }
        
        
    }
}