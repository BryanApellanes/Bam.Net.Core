using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Bam.Net.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Schema;
using YamlDotNet.Serialization;

namespace Bam.Net.Schema.Json
{
    public class JSchemaProperty
    {
        public JSchemaProperty(JSchema propertyType)
        {
            JSchemaOfProperty = propertyType;
        }

        protected ILogger Logger => DeclaringClass.ClassManager.Logger;
        public string DeclaringClassName => DeclaringClass.ClassName;
        
        [Exclude]
        [XmlIgnore]
        [JsonIgnore]
        [YamlIgnore]
        public JSchemaClass DeclaringClass { get; set; }
        
        public bool? IsEnum => ClassOfProperty?.IsEnum;

        public IEnumerable<string> GetEnumNames()
        {
            return ClassOfProperty.GetEnumNames();
        }

        public string[] EnumNames
        {
            get
            {
                return IsEnum != null && IsEnum.Value ? GetEnumNames().ToArray() : new string[] { };
            }
        }
        
        public bool IsArray => JSchemaOfProperty.Type == JSchemaType.Array;
        
        [Exclude]
        [JsonIgnore]
        [XmlIgnore]
        [YamlIgnore]
        public JSchema JSchemaOfArrayItems => JSchemaOfProperty?.Items?.FirstOrDefault();
        
        public JSchemaClass ClassOfArrayItems
        {
            get
            {
                JSchemaClass jSchemaClass = JSchemaOfArrayItems != null
                    ? new JSchemaClass(JSchemaOfArrayItems, DeclaringClass.ClassManager)
                    : null;

                if (jSchemaClass != null && string.IsNullOrEmpty(jSchemaClass.ClassName))
                {
                    Logger?.Warning("Unable to determine class name for array items of property ({0}).({1}) using class name ({1})", DeclaringClass.ClassName, PropertyName);
                    jSchemaClass.ClassName = PropertyName;
                }
                return jSchemaClass;
            }
        }

        [Exclude]
        [JsonIgnore]
        [XmlIgnore]
        [YamlIgnore]
        public JSchema JSchemaOfProperty { get; }

        /// <summary>
        /// The class of this property.  May be null if this property is not an object.
        /// </summary>
        [Exclude]
        [JsonIgnore]
        [XmlIgnore]
        [YamlIgnore]
        public JSchemaClass ClassOfProperty
        {
            get
            {
                if (Type == JSchemaType.Object)
                {
                    JSchemaClass result = new JSchemaClass(JSchemaOfProperty, DeclaringClass.ClassManager);
                    if (string.IsNullOrEmpty(result.ClassName))
                    {
                        Logger?.Warning("Unable to determine class name of property ({0}).({1}) using class name ({1})", DeclaringClass.ClassName, PropertyName);
                        result.ClassName = PropertyName;
                    }

                    return result;
                }

                return null;
            }  
        } 
        
        [JsonConverter(typeof(StringEnumConverter))]
        public JSchemaType? Type => JSchemaOfProperty.Type;

        public string PropertyClassName => Type == JSchemaType.Object ? ClassOfProperty?.ClassName : null;

        public string PropertyName { get; set; }

        public override string ToString()
        {
            return PropertyName;
        }
    }
}