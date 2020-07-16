using System;

namespace Bam.Net.Server.PathHandlers.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PostAttribute: PathAttribute
    {
        public PostAttribute(string path) : base(path)
        {
        }
    }
}