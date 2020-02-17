using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Bam.Net.Server.PathHandlers;
using Bam.Net.Server.PathHandlers.Attributes;
using Bam.Net.Presentation.Tests.PathHandler;
using bam.net.presentation.tests.PathHandlerTests;
using Bam.Net.Server;
using Bam.Net.ServiceProxy;
using Bam.Net.Testing;
using Bam.Net.Testing.Unit;
using Bam.Net.Web;
using NSubstitute;

namespace Bam.Net.Presentation.Tests
{
    [Serializable]
    public class PathHandlerTests: CommandLineTestInterface
    {
        [UnitTest]
        [TestGroup("RouteHandlers")]
        public void CanResolveHandlerName()
        {
            TestRouteHandlerManager routeHandlerManager = GetRouteHandlerManager();
            string handlerName = routeHandlerManager.CallGetHandlerName("https://localhost:8080/test/1/health");
            Expect.AreEqual("test", handlerName);
        }

        [UnitTest]
        [TestGroup("RouteHandlers")]
        public void CanResolveHandlerMethod()
        {
            bool? thrown = false;
            TestRouteHandlerManager routeHandlerManager = GetRouteHandlerManager();
            routeHandlerManager
                .ScanForRouteHandlers(".")
                .ContinueWith((task, state) =>
                {
                    try
                    {
                        MethodInfo handlerMethod = routeHandlerManager.CallResolveHandlerMethod("https://localhost:8080/test/1/health", out Dictionary<string, string> parameters);
                        MethodInfo expected = typeof(TestHandler).GetMethod("GetHealth", new Type[] {typeof(string)});
                        (handlerMethod.Name.Equals(expected.Name)).IsTrue("Method names didn't match");
                        (handlerMethod.GetParameters().Length == expected.GetParameters().Length).IsTrue("Parameter count didn't match");
                    }
                    catch (Exception ex)
                    {
                        thrown = true;
                        OutLineFormat("Exception asserting expectations: {0}", ConsoleColor.Magenta, ex.Message);
                    }
                }, routeHandlerManager)
                .Wait();

            thrown.Value.IsFalse();
        }
        
        [UnitTest]
        [TestGroup("RouteHandlers")]
        public void CanGetPathAttributeMethods()
        {
            MethodInfo[] methods = typeof(TestHandler).GetMethods().Where(mi => mi.HasCustomAttributeOfType<PathAttribute>(true)).ToArray();
            Expect.AreEqual(8, methods.Length);

            ValidateAttributeType<GetAttribute>(methods, nameof(TestHandler.GetHealth));
            ValidateAttributeType<GetAttribute>(methods, nameof(TestHandler.ReadObjectType));
            ValidateAttributeType<PostAttribute>(methods, nameof(TestHandler.PostObjectType));
            ValidateAttributeType<PutAttribute>(methods, nameof(TestHandler.PutObjectType));
        }

        private void ValidateAttributeType<T>(MethodInfo[] methods, string methodName) where T: PathAttribute
        {
            MethodInfo getHealth = methods.FirstOrDefault(m => m.Name.Equals(methodName));
            getHealth.HasCustomAttributeOfType<PathAttribute>(true, out PathAttribute getHealthPathAttribute);
            Expect.AreEqual(typeof(T), getHealthPathAttribute.GetType());
        }
        
        // RouteHandlerManager should register route handlers
        //     - scan a specified folder for an applications route handlers given the AppConf
        [UnitTest]
        [TestGroup("RouteHandlers")]
        public void CanScanAFolderForRouteHandlers()
        {
            RouteHandlerManager routeHandlerManager = GetRouteHandlerManager();
            bool? thrown = false;
            routeHandlerManager
                .ScanForRouteHandlers(".")
                .ContinueWith((task, state) =>
                {
                    try
                    {
                        (state is RouteHandlerManager rhManager).IsTrue();
                        RouteHandlerManager mgr = (RouteHandlerManager) state;
                        string routeHandlerName = typeof(TestHandler).GetCustomAttributeOfType<HandlesAttribute>().Name;
                        (mgr.RouteHandlers.Count == 1).IsTrue();
                        mgr.RouteHandlers.ContainsKey("/test");
                        Expect.AreEqual(typeof(Dictionary<PathAttribute, MethodInfo>), mgr.RouteHandlers["/test"].GetType());
                        Dictionary<PathAttribute, MethodInfo> testHandlers = mgr.RouteHandlers["/test"];
                        Expect.AreEqual(8, testHandlers.Count);
                    }
                    catch (Exception ex)
                    {
                        thrown = true;
                        OutLineFormat("Exception asserting expectations: {0}", ConsoleColor.Magenta, ex.Message);
                    }
                }, routeHandlerManager)
                .Wait();
            
            thrown.Value.IsFalse();
            Thread.Sleep(3);
            OutLine("scan test complete", ConsoleColor.Cyan);
        }
        
        //     - register each route handler internally
        [UnitTest]
        [TestGroup("RouteHandlers")]
        public void CanRegisterType()
        {
            RouteHandlerManager routeHandlerManager = GetRouteHandlerManager();
            routeHandlerManager.RegisterType(typeof(TestHandler));
            string routeHandlerName = typeof(TestHandler).GetCustomAttributeOfType<HandlesAttribute>().Name;
            /*routeHandlerManager.RouteHandlers
                .ContainsKey(HttpVerbs.Get)
                .IsTrue("route handler manager did not contain GET route handler");
            
            routeHandlerManager.RouteHandlers[HttpVerbs.Get]
                .ContainsKey("test")
                .IsTrue("GET handlers did not contain 'test' handler");*/
        }

        [UnitTest]
        [TestGroup("RouteHandlers")]
        public void ShouldResolveRouteHandler()
        {
            string url = "http://localhost:8080/test/1/health";
            TestRouteHandlerManager routeHandlerManager = (TestRouteHandlerManager)GetRouteHandlerManager();
            object handler = routeHandlerManager.CallResolveHandlerMethod(url, out Dictionary<string, string> parameters);
            Expect.AreEqual(typeof(TestHandler), handler.GetType());
        }
        
        // RouteHandlerManager should resolve Url to an execution request
        [UnitTest]
        [TestGroup("RouteHandlers")]
        public void ShouldResolveUrlToExecutionRequest()
        {
            string url = "http://localhost:8080/test/1/health";
            RouteHandlerManager routeHandlerManager = GetRouteHandlerManager();
            ExecutionRequest executionRequest = routeHandlerManager.ResolveRequest(url);

            Expect.AreEqual(typeof(TestHandler), executionRequest.TargetType);
            Expect.AreEqual(1, executionRequest.ParameterInfos.Length);
        }
        
        // BamServer should short circuit the response pipeline to determine if it should use the RouteManager or the Responder track
        [UnitTest]
        [TestGroup("RouteHandlers")]
        public void ShouldExecuteHandler()
        {
            string url = "http://localhost:8080/test/1/health";
            BamServer server = new BamServer(BamConf.Load(nameof(ShouldExecuteHandler)));
            IHttpContext testHttpContext = Substitute.For<IHttpContext>();
            IRequest testRequest = Substitute.For<IRequest>();
            testRequest.Url.Returns(new Uri(url));
            
            IResponse testResponse = Substitute.For<IResponse>();
            testHttpContext.Request.Returns(testRequest);
            testHttpContext.Response.Returns(testResponse);
            
            server.HandleRequest(testHttpContext);
            
            throw new NotImplementedException("This test is not complete");
        }

        private TestRouteHandlerManager GetRouteHandlerManager()
        {
            return new TestRouteHandlerManager();
        }
    }
}