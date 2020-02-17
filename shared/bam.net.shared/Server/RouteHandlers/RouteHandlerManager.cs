using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bam.Net.Logging;
using Bam.Net.Server.PathHandlers.Attributes;
using Bam.Net.ServiceProxy;
using Bam.Net.Web;

namespace Bam.Net.Server
{
    [Serializable]
    public class RouteHandlerManager: Loggable
    {
        public RouteHandlerManager(ILogger logger = null)
        {
            RouteHandlers = new Dictionary<string, Dictionary<PathAttribute, MethodInfo>>();
            Logger = logger ?? Log.Default;
        }

        private ILogger _logger;

        public ILogger Logger
        {
            get => _logger;
            set
            {
                _logger = value;
                Subscribe(_logger);
            }
        }

        public Dictionary<string, Dictionary<PathAttribute, MethodInfo>> RouteHandlers { get; private set; }

        protected void AddHandler(string name, PathAttribute pathAttribute, MethodInfo method)
        {
            if (!RouteHandlers.ContainsKey(name))
            {
                RouteHandlers.Add(name, new Dictionary<PathAttribute, MethodInfo>());
            }

            if (!RouteHandlers[name].ContainsKey(pathAttribute))
            {
                RouteHandlers[name].Add(pathAttribute, method);
            }
        }
        
        public Task ScanForRouteHandlers(string directoryPath)
        {
            return Task.Run(() =>
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                Parallel.ForEach(directoryInfo.GetFiles("*.dll", SearchOption.AllDirectories), fileInfo =>
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFile(fileInfo.FullName);
                        foreach (Type type in assembly.GetTypes())
                        {
                            if (type.HasCustomAttributeOfType<HandlesAttribute>(out HandlesAttribute handlesAttribute))
                            {
                                foreach (MethodInfo method in type.GetMethods())
                                {
                                    if (method.HasCustomAttributeOfType<PathAttribute>(true, out PathAttribute pathAttribute))
                                    {
                                        AddHandler(handlesAttribute.Name, pathAttribute, method);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Warn("Error scanning for route handlers: {0}", ex.Message);
                    }
                });
            });
        }
        
        public bool RegisterType(Type type)
        {
            throw new NotImplementedException();
        }
        
        public bool CanHandle(string url, out ExecutionRequest executionRequest)
        {
            throw new NotImplementedException();
        }
        
        public ExecutionRequest ResolveRequest(string url)
        {
            throw new NotImplementedException();
        }

        protected virtual MethodInfo ResolveHandlerMethod(string url, out Dictionary<string, string> parameters)
        {
            string handlerName = GetHandlerName(url);
            MethodInfo result = null;
            Dictionary<string, string> parameterResult = null;
            Uri uri = new Uri(url);
            string parameterPath = uri.PathAndQuery.TruncateFront($"/{handlerName}".Length);
            if (RouteHandlers.ContainsKey(handlerName))
            {
                Dictionary<PathAttribute, MethodInfo> handlerMethods = RouteHandlers[handlerName];
                HashSet<MethodInfo> candidates = new HashSet<MethodInfo>();
                foreach (PathAttribute pathAttribute in handlerMethods.Keys)
                {
                    MethodInfo method = handlerMethods[pathAttribute];
                    HashSet<string> parameterNames = new HashSet<string>(method.GetParameters().Select(p=>p.Name).ToArray());
                    Dictionary<string, string> methodParameters = pathAttribute.ParsePath(parameterPath);
                    if (methodParameters.Count == method.GetParameters().Length)
                    {
                        // if all the parameters are in the keys then add
                        bool oneIsMissing = false;
                        foreach (string parameterName in parameterNames)
                        {
                            if (!methodParameters.ContainsKey(parameterName))
                            {
                                oneIsMissing = true;
                            }
                        }

                        if (!oneIsMissing)
                        {
                            parameterResult = methodParameters;
                            result = method;
                            break;
                        }
                    }
                }
            }

            parameters = parameterResult;
            return result;
        }

        protected virtual string GetHandlerName(string url)
        {
            Uri uri = new Uri(url);
            return uri.PathAndQuery.Split("/", StringSplitOptions.RemoveEmptyEntries).First();
        }
    }
}