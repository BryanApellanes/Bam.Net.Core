using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public class JSchemaProperty
    {
        public JSchemaProperty(JSchema propertyType)
        {
            JSchemaOfProperty = propertyType;
        }
        
        [Exclude]
        [XmlIgnore]
        [JsonIgnore]
        public JSchemaClass DeclaringClass { get; set; }
        
        public bool IsEnum => ClassOfProperty.IsEnum;

        public IEnumerable<string> GetEnumNames()
        {
            return ClassOfProperty.GetEnumNames();
        }

        public string[] EnumNames
        {
            get
            {
                return IsEnum ? GetEnumNames().ToArray() : new string[] { };
            }
        }
        
        public bool IsArray => JSchemaOfProperty.Type == JSchemaType.Array;
        
        [Exclude]
        [JsonIgnore]
        [XmlIgnore]
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
                    jSchemaClass.ClassName = PropertyName;
                }
                return jSchemaClass;
            }
        }

        [Exclude]
        [JsonIgnore]
        [XmlIgnore]
        public JSchema JSchemaOfProperty { get; }

        [Exclude]
        [JsonIgnore]
        [XmlIgnore]
        public JSchemaClass ClassOfProperty => new JSchemaClass(JSchemaOfProperty, DeclaringClass.ClassManager);

        [JsonConverter(typeof(StringEnumConverter))]
        public JSchemaType? Type => JSchemaOfProperty.Type;

        public string PropertyClassName => Type == JSchemaType.Object ? ClassOfProperty.ClassName : null;

        public string PropertyName { get; set; }

        public override string ToString()
        {
            return PropertyName;
        }
    }
}