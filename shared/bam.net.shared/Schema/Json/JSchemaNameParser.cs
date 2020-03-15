using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public class JSchemaNameParser
    {
        private List<string> _classNameProperties;
        public JSchemaNameParser()
        {
            _classNameProperties = new List<string> {"class", "className"};
            MungeClassName = (s) => s.PascalCase(true, " ", "_", "/", "\\", "-", ".", ",");
            ExtractJSchemaClassName = schema =>
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
            ExtractJObjectClassName = jObject =>
            {
                if (jObject["type"].Equals("object"))
                {
                    foreach (string classNameProperty in _classNameProperties)
                    {
                        if (!string.IsNullOrEmpty(jObject[classNameProperty].ToString()))
                        {
                            return MungeClassName(jObject[classNameProperty].ToString());
                        }
                    }
                }

                return MungeClassName(jObject.ToJson().Sha256());
            };
            MungeEnumName = (s) => s.PascalCase(true, " ", ".", ",").Replace("-", "_").Replace("/", "_").Replace("\\", "_");
            ParsePropertyName = (s) => s.PascalCase(true, " ", ".", ",").Replace("-", "_").Replace("/", "_").Replace("\\", "_");
        }

        public Func<string, string> MungeClassName { get; set; }  
        public Func<JSchema, string> ExtractJSchemaClassName { get; set; } 
        public Func<JObject, string> ExtractJObjectClassName { get; set; }
        public Func<string, string> MungeEnumName { get; set; }
        public Func<string, string> ParsePropertyName { get; set; }
    }
}