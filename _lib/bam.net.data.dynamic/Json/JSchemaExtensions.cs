using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public static class JSchemaExtensions
    {
        public static bool IsObject(this JSchema jSchema)
        {
            if (jSchema?.Type == null)
            {
                return false;
            }
            return jSchema.Type.ToString().Equals("object", StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsEnum(this JSchema jSchema)
        {
            return IsEnum(jSchema, out string[] ignore);
        }

        public static bool IsEnum(this JSchema jSchema, out string[] enumValues)
        {
            enumValues = jSchema.Enum?.Select(v => v.ToString()).ToArray();
            return jSchema.Enum != null && enumValues.Length > 0;
        }

        /// <summary>
        /// Determines if the specified JSchema has a 'type' property.
        /// </summary>
        /// <param name="jSchema"></param>
        /// <returns></returns>
        public static bool HasTypeProperty(this JSchema jSchema)
        {
            return HasProperty(jSchema, "type");
        }
        
        public static bool HasDefinitions(this JSchema jSchema)
        {
            return HasDefinitions(jSchema, out JSchema ignore);
        }

        public static bool HasDefinitions(this JSchema jSchema, out JSchema definitions)
        {
            bool result = HasProperty(jSchema, "definitions", out object definitionz);
            definitions = (JSchema)definitionz;
            return result;
        }
        
        /// <summary>
        /// Determines if the specified JSchema has the specified property in its
        /// ExtensionData dictionary property.
        /// </summary>
        /// <param name="jSchema"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool HasProperty(this JSchema jSchema, string propertyName)
        {
            return HasProperty(jSchema, propertyName, out object ignore);
        }
        
        /// <summary>
        /// Determines if the specified JSchema has the specified property in its
        /// ExtensionData dictionary property.
        /// </summary>
        /// <param name="jSchema"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HasProperty(this JSchema jSchema, string propertyName, out object value)
        {
            bool result = (bool) (jSchema?.ExtensionData.ContainsKey(propertyName));
            value = null;
            if (result)
            {
                value = jSchema.ExtensionData[propertyName];
            }

            return result;
        }

        /// <summary>
        /// Parses all the values in the specified schema (as Dictionary&lt;object, object&gt;) to determine
        /// if they are parseable as integers or booleans, converting the value if so. 
        /// </summary>
        /// <param name="schema"></param>
        public static void  ConvertJSchemaPropertyTypes(this Dictionary<object, object> schema)
        {
            schema.Keys.BackwardsEach(key =>
            {
                if (schema[key] is string value)
                {
                    if (int.TryParse(value, out int intValue))
                    {
                        schema[key] = intValue;
                    } 
                    else if (double.TryParse(value, out double doubleValue))
                    {
                        schema[key] = doubleValue;
                    }
                    else if (bool.TryParse(value, out bool boolValue))
                    {
                        schema[key] = boolValue;
                    }
                    else if (value.IsAffirmative())
                    {
                        schema[key] = true;
                    }
                    else if (value.IsNegative())
                    {
                        schema[key] = false;
                    }
                }
                else if (schema[key] is Dictionary<object, object> dictionary)
                {
                    ConvertJSchemaPropertyTypes(dictionary);
                }
            });
        }
        
        public static void ConvertJSchemaPropertyTypes(this JObject root)
        {
            if (root == null)
            {
                return;
            }
            
            foreach (JProperty property in root.Properties())
            {
                string value = root.Property(property.Name).Value.ToString();
                if (int.TryParse(value, out int intValue))
                {
                    root[property.Name] = intValue;
                }
                else if (bool.TryParse(value, out bool boolValue))
                {
                    root[property.Name] = boolValue;
                }
                else if (value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                {
                    root[property.Name] = true;
                }
                else if (value.Equals("false", StringComparison.InvariantCultureIgnoreCase))
                {
                    root[property.Name] = true;
                }
                else if (root[property.Name] is JObject jObject)
                {
                    ConvertJSchemaPropertyTypes(jObject);
                }
            }
        }
    }
}