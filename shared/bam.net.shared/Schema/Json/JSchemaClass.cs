using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Bam.Net.Application.Json;
using Microsoft.CodeAnalysis.CSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public class JSchemaClass
    {
        public JSchemaClass(JSchemaClassManager classManager, string message = null)
        {
            ClassManager = classManager;
            Message = message;
        }

        public JSchemaClass(JSchema jSchema, JSchemaClassManager classManager) : this(classManager)
        {
            JSchema = jSchema;
            ClassName = classManager.ExtractClassName(JSchema);
        }

        protected JSchemaClassManager ClassManager { get; private set; }
        public string Message { get; set; }

        private string _className;

        public string ClassName
        {
            get
            {
                if (string.IsNullOrEmpty(_className))
                {
                    _className = ClassManager.ExtractClassName(JSchema);
                }

                return _className;
            }
            set => _className = value;
        }

        public bool IsEnum => JSchema.Enum?.Count > 0;

        public IEnumerable<string> GetEnumNames()
        {
            foreach (JToken token in JSchema.Enum)
            {
                yield return ClassManager.MungeEnumName(token.ToString());
            }
        }

        [Exclude]
        [XmlIgnore]
        [JsonIgnore]
        public JSchema JSchema { get; set; }

        public IDictionary<string, JSchemaProperty> Properties
        {
            get
            {
                Dictionary<string, JSchemaProperty> properties = new Dictionary<string, JSchemaProperty>();
                JSchema?.Properties?.Keys.Each(propertyName => properties.Add(propertyName, new JSchemaProperty(JSchema.Properties[propertyName]) {PropertyName = propertyName}));

                return properties;
            }
        }

        /// <summary>
        /// Retrieve JSchemaClasses from the "definitions" section of the specified JSchema.
        /// </summary>
        /// <param name="jSchema"></param>
        /// <param name="manager"></param>
        /// <returns></returns>
        public static IEnumerable<JSchemaClass> FromDefinitions(JSchema jSchema, JSchemaClassManager manager)
        {
            if (jSchema.ExtensionData.ContainsKey("definitions"))
            {
                foreach(JProperty prop in ((JObject)jSchema.ExtensionData["definitions"]).Properties())
                {
                    yield return new JSchemaClass(JSchema.Parse(prop.Value.ToJson(), manager.JSchemaResolver), manager)
                    {
                        ClassName = prop.Name
                    };
                };
            }
        }
    }
}