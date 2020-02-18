using Bam.Net.CommandLine;
using Bam.Net.Logging;
using Bam.Net.Server.PathHandlers.Attributes;
using Bam.Net.Server;

namespace Bam.Net.Presentation.Tests.PathHandler
{ 
    public class TestData
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    
    [HandlerFor("test")]
    public class TestHandler
    {
        public TestHandler()
        {
            ConsoleLogger logger = new ConsoleLogger {AddDetails = false, UseColors = true};
            logger.StartLoggingThread();
            Logger = logger;
        }
        
        public ILogger Logger { get; set; }
        
        [Get("/{version}/health")]
        public object GetHealth(string version)
        {
            return $"{version} = OK";
        }

        [Get("/{objectType}/get")]
        public object ReadObjectType(string objectType)
        {
            return $"ReadObjectType: objectType = {objectType}";
        }
        
        [Get("/{objectType}/get/{id}")]
        public object ReadObjectType(string objectType, string id)
        {
            return $"ReadObjectType: objectType = {objectType}";
        }
        
        [Get("/{objectType}/get/{objectProperty}")]
        public object ReadObjectProperty(string objectType, string objectProperty)
        {
            return $"ReadObjectType: objectType = {objectType}, objectProperty = {objectProperty}";
        }
        
        [Get("/{objectType}/{objectProperty}/get?filter={filter}")]
        public object ReadObjectProperty(string objectType, string objectProperty, string filter)
        {
            return $"ReadObjectType: objectType = {objectType}, objectProperty = {objectProperty}, filter = {filter}";
        }

        [Post("/{objectType}/post")]
        public object PostObjectType(string objectType, TestData value)
        {
            return $"PostObjectType: \r\n\tobjectType = {objectType}, \r\n\tvalue = {value.PropertiesToString()}";
        }
        
        /*[Post("/{objectType}/post")]
        public T PostObjectType<T>(string objectType, T value)
        {
            Logger.Info($"PostObjectType: \r\n\tobjectType = {objectType}, \r\n\tvalue = {value.PropertiesToString()}");
            return value;
        }*/

        [Put("/{objectType}/put")]
        public object PutObjectType(string objectType, object value)
        {
            return $"PutObjectType: \r\n\tobjectType = {objectType}, \r\n\tvalue = {value.PropertiesToString()}";
        }
        
        [Patch("/{objectType}/patch")]
        public object PatchObjectType(string objectType, object value)
        {
            return $"PatchObjectType: \r\n\tobjectType = {objectType}, \r\n\tvalue = {value.PropertiesToString()}";
        }
        
        [Delete("/{objectType}/delete")]
        public object DeleteObjectType(string objectType, object value)
        {
            return $"DeleteObjectType: \r\n\tobjectType = {objectType}, \r\n\tvalue = {value.PropertiesToString()}";
        }

        public object ReturnNull()
        {
            return null;
        }

        public object ReturnObject()
        {
            return new object();
        }

        public T Return<T>() where T: new()
        {
            return new T();
        }
    }
}