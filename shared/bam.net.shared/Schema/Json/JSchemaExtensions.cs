using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public static class JSchemaExtensions
    {
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