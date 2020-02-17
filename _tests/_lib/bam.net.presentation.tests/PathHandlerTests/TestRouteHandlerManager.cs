using System.Collections.Generic;
using System.Reflection;
using Bam.Net.Server.PathHandlers;
using Bam.Net.Server;

namespace bam.net.presentation.tests.PathHandlerTests
{
    public class TestRouteHandlerManager: RouteHandlerManager
    {
        public MethodInfo CallResolveHandlerMethod(string url, out Dictionary<string, string> parameters)
        {
            return ResolveHandlerMethod(url, out parameters);
        }

        public string CallGetHandlerName(string url)
        {
            return GetHandlerName(url);
        }
    }
}