using System;
using System.Linq;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public static class JSchemaExtensions
    {
        public static bool IsObject(this JSchema jSchema)
        {
            Args.ThrowIfNull(jSchema?.Type, "jSchema.Type");
            
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
        
        public static bool HasJSchemaProperty(this JSchema jSchema, string propertyName)
        {
            return HasJSchemaProperty(jSchema, propertyName, out object ignore);
        }
        
        public static bool HasJSchemaProperty(this JSchema jSchema, string propertyName, out object value)
        {
            bool result = (bool) (jSchema?.ExtensionData.ContainsKey(propertyName));
            value = null;
            if (result)
            {
                value = jSchema.ExtensionData[propertyName];
            }

            return result;
        }
    }
}