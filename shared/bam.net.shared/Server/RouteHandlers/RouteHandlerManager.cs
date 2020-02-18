using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bam.Net.Data;
using Bam.Net.Logging;
using Bam.Net.Server.PathHandlers.Attributes;
using Bam.Net.ServiceProxy;
using Bam.Net.Web;
using ParameterInfo = System.Reflection.ParameterInfo;

namespace Bam.Net.Server
{
    [Serializable]
    public class RouteHandlerManager: Loggable
    {
        public RouteHandlerManager(ILogger logger = null)
        {
            TypeConverters = new Dictionary<ParameterInfo, Func<string, Type, object>>();
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

        /// <summary>
        /// Functions used to convert a string into an object of a specified type.  Used to convert method arguments. 
        /// </summary>
        public Dictionary<ParameterInfo, Func<string, Type, object>> TypeConverters { get; set; }
        
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
                            if (type.HasCustomAttributeOfType<HandlerForAttribute>(out HandlerForAttribute handlesAttribute))
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

        public bool CanHandle(IRequest request, out ExecutionRequest executionRequest)
        {
            throw new NotImplementedException();
        }
        
        public ExecutionRequest ResolveRequest(string url)
        {
            throw new NotImplementedException();
        }

        protected virtual MethodInfo ResolveHandlerMethod(IRequest request, out Dictionary<string, object> objectParameters)
        {
            MethodInfo method = ResolveHandlerMethod(request.Url.ToString(), out Dictionary<string, string> arguments);
            if (method.HasObjectParameters(out ParameterInfo[] objectTypeParameters))
            {
                objectParameters = ConvertArgumentTypes(request, method, objectTypeParameters.ToDictionary(pi=> pi.Name), arguments);
            }
            else
            {
                objectParameters = new Dictionary<string, object>();
                arguments.Keys.Each(key => arguments.Add(key, arguments[key]));
            }

            return method;
        }
        
        protected virtual MethodInfo ResolveHandlerMethod(string url, out Dictionary<string, string> arguments)
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

            arguments = parameterResult;
            return result;
        }

        protected virtual string GetHandlerName(string url)
        {
            Uri uri = new Uri(url);
            return uri.PathAndQuery.Split("/", StringSplitOptions.RemoveEmptyEntries).First();
        }

        protected Dictionary<string, object> ConvertArgumentTypes(IRequest request, MethodInfo methodInfo, Dictionary<string, ParameterInfo> parameterInfos, Dictionary<string, string> routeArgs)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (string key in routeArgs.Keys)
            {
                ParameterInfo parameterInfo = parameterInfos[key];
                if (TypeConverters.ContainsKey(parameterInfo))
                {
                    result.Add(key, TypeConverters[parameterInfo](key, parameterInfo.ParameterType));
                }
            }
            
            return result;
        }
    }
}