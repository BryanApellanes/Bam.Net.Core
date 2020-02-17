using System;
using System.Collections.Generic;
using Bam.Net.Web;

namespace Bam.Net.Server.PathHandlers.Attributes
{
    /// <summary>
    /// Base attribute used to adorn methods that execute in response to url paths.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public abstract class PathAttribute: Attribute
    {
        public PathAttribute(string path)
        {
            Path = path;
            Verb = HttpVerbs.Get;
        }
        
       public string Path { get; set; }
       public HttpVerbs Verb { get; set; }

       public Dictionary<string, string> ParsePath(string uriPath)
       {
           RouteParser routeParser = new RouteParser(Path);
           return routeParser.ParseRouteInstance(uriPath);
       }

       public override string ToString()
       {
           return Path;
       }
    }
}