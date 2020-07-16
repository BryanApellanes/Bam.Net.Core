using System;
using Bam.Net.Web;

namespace Bam.Net.Server.PathHandlers.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class DeleteAttribute: PathAttribute
    {
        public DeleteAttribute(string path) : base(path)
        {
            Verb = HttpVerbs.Delete;
        }
    }
}