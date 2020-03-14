using System;
using MongoDB.Bson;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public class JSchemaNameParser
    {
        public JSchemaNameParser()
        {
            MungeClassName = (s) => s.PascalCase(true, " ", "_", "/", "\\", "-", ".", ",");
            ExtractClassName = schema =>
            {
                if (!string.IsNullOrEmpty(schema?.Id?.ToString()))
                {
                    return MungeClassName(schema.Id.ToString());
                }
                else if (!string.IsNullOrEmpty(schema?.Title))
                {
                    return MungeClassName(schema.Title);
                }

                return MungeClassName(schema.ToJson().Sha256());
            };
        }

        public Func<string, string> MungeClassName { get; set; }  
        public Func<JSchema, string> ExtractClassName { get; set; } 
        public Func<string, string> MungeEnumName { get; set; }
        public Func<string, string> ParsePropertyName { get; set; }
    }
}