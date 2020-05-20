using System;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public class JSchemaLoadResult
    {
        public JSchemaLoadResult(string path, JSchema jSchema)
        {
            Path = path;
            JSchema = jSchema;
        }

        public JSchemaLoadResult(string path, Exception exception)
        {
            Path = path;
            Exception = exception;
        }

        public JSchemaLoadResult(JSchema jSchema, Exception exception)
        {
            Path = ".";
            JSchema = jSchema;
            Exception = exception;
        }
        
        public string Path { get; set; }
        public JSchema JSchema { get; set; }
        public bool Success => Exception == null;
        public string Message => Exception?.Message;
        public string StackTrace => Exception?.StackTrace;
        private Exception Exception { get; set; } 
    }
}