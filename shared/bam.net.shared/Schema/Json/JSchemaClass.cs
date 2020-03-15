using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Bam.Net.Application.Json;
using Bam.Net.Data;
using Microsoft.CodeAnalysis.CSharp;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public class JSchemaClass
    {
        public JSchemaClass(JSchemaClassManager classManager, string filePath = null)
        {
            ClassManager = classManager;
            FilePath = filePath;
        }

        public JSchemaClass(JSchema jSchema, JSchemaClassManager classManager, string filePath = null) : this(classManager, filePath)
        {
            JSchema = jSchema;
            ClassName = classManager.ExtractJSchemaClassName(JSchema);
        }

        protected internal JSchemaClassManager ClassManager { get; private set; }
        public string FilePath { get; set; }

        private string _className;
        public string ClassName
        {
            get
            {
                if (string.IsNullOrEmpty(_className))
                {
                    _className = ClassManager.ExtractJSchemaClassName(JSchema);
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
                if (token != null)
                {
                    yield return ClassManager.MungeEnumName(token.ToString());
                }
            }
        }
        public string[] EnumNames
        {
            get
            {
                return IsEnum ? GetEnumNames().ToArray() : new string[] { };
            }
        }
        
        [Exclude]
        [XmlIgnore]
        [JsonIgnore]
        public JSchema JSchema { get; private set; }

        public IEnumerable<JSchemaProperty> Properties
        {
            get
            {
                foreach (string propertyName in JSchema?.Properties?.Keys)
                {
                    yield return new JSchemaProperty(JSchema.Properties[propertyName]){DeclaringClass = this, PropertyName = ClassManager.ParsePropertyName(propertyName)};
                }
            }
        }

        [Exclude]
        [XmlIgnore]
        [JsonIgnore]
        public IEnumerable<JSchemaProperty> ValueProperties
        {
            get
            {
                return Properties.Where(p =>
                    p.JSchemaOfProperty.Type == JSchemaType.String || p.JSchemaOfProperty.Type == JSchemaType.Integer ||
                    p.JSchemaOfProperty.Type == JSchemaType.Number || p.JSchemaOfProperty.Type == JSchemaType.Boolean);
            }
        }

        [Exclude]
        [XmlIgnore]
        [JsonIgnore]
        public IEnumerable<JSchemaProperty> EnumProperties
        {
            get { return Properties.Where(p => p.ClassOfProperty.IsEnum); }
        }
        
        [Exclude]
        [XmlIgnore]
        [JsonIgnore]
        public IEnumerable<JSchemaProperty> ObjectProperties
        {
            get
            {
                return Properties.Where(p => p.JSchemaOfProperty.Type == JSchemaType.Object || p.JSchemaOfProperty.Type == JSchemaType.Null || p.JSchemaOfProperty.Type == JSchemaType.None);
            }
        }

        [Exclude]
        [XmlIgnore]
        [JsonIgnore]
        public IEnumerable<JSchemaProperty> ArrayProperties => PropertiesOfType(JSchemaType.Array);

        public IEnumerable<JSchemaProperty> PropertiesOfType(JSchemaType type)
        {
            return Properties.Where(p => p.JSchemaOfProperty.Type == type);
        }

        public override bool Equals(object obj)
        {
            if (obj is JSchemaClass other)
            {
                if (other.JSchema != null && this.JSchema != null)
                {
                    return other.JSchema.ToJson().Equals(this.JSchema.ToJson());
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (JSchema != null)
            {
                return JSchema.ToJson().GetHashCode();
            }

            return base.GetHashCode();
        }

        public override string ToString()
        {
            return ClassName;
        }

        /// <summary>
        /// Retrieve JSchemaClasses from the "definitions" section of the specified JSchema.
        /// </summary>
        /// <param name="jSchema"></param>
        /// <param name="manager"></param>
        /// <returns></returns>
        public static IEnumerable<JSchemaClass> FromDefinitions(JSchema jSchema, JSchemaClassManager manager)
        {
            return FromPropertyObjects(jSchema, manager, "definitions");
        }

        public static IEnumerable<JSchemaClass> FromPropertyObjects(JSchema jSchema, JSchemaClassManager manager, string extensionDataKey)
        {
            if (jSchema.ExtensionData.ContainsKey(extensionDataKey))
            {
                foreach(JProperty prop in ((JObject)jSchema.ExtensionData[extensionDataKey]).Properties())
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