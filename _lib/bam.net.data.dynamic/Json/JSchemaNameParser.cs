using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public class JSchemaNameParser
    {
        private readonly List<Func<JSchema, string>> _classNameExtractors;
        private readonly List<string> _classNameProperties;
        public JSchemaNameParser()
        {
            _classNameExtractors = new List<Func<JSchema, string>>();
            _classNameProperties = new List<string> {"class", "className"};
            MungeClassName = (s) => s.PascalCase(true, " ", "_", "/", "\\", "-", ".", ",", "@");
            ExtractJSchemaClassName = schema =>
            {
                string result = string.Empty;
                string extracted = ExtractClassName(schema);
                if (!string.IsNullOrEmpty(extracted))
                {
                    result = MungeClassName(extracted);
                }
                else if (!string.IsNullOrEmpty(schema?.Id?.ToString()))
                {
                    result = MungeClassName(schema.Id.ToString());
                }
                else if (!string.IsNullOrEmpty(schema?.Title))
                {
                    result = MungeClassName(schema.Title);
                }

                if (string.IsNullOrEmpty(result))
                {
                    result = MungeClassName(schema.ToJson().Sha256());
                }

                return result;
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
            MungePropertyName = (s) => s.PascalCase(true, " ", ".", ",").Replace("-", "_").Replace("/", "_").Replace("\\", "_");
        }

        public Func<string, string> MungeClassName { get; set; }  
        public Func<JSchema, string> ExtractJSchemaClassName { get; set; } 
        public Func<JObject, string> ExtractJObjectClassName { get; set; }
        public Func<string, string> MungeEnumName { get; set; }
        public Func<string, string> MungePropertyName { get; set; }

        public JSchemaNameParser AddClassNameProperty(string classNameProperty)
        {
            _classNameProperties.Add(classNameProperty);
            return this;
        }
        public JSchemaNameParser AddClassNameExtractor(Func<JSchema, string> extractor)
        {
            _classNameExtractors.Add(extractor);
            return this;
        }

        public string ExtractClassName(JSchema jSchema)
        {
            string className = string.Empty;
            foreach(Func<JSchema, string> func in _classNameExtractors)
            {
                if (!string.IsNullOrEmpty(className))
                {
                    break;
                }

                className = func(jSchema);
            }

            return className;
        }
    }
}