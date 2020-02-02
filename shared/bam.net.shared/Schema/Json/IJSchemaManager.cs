using System.Collections.Generic;
using Bam.Net.Data.Schema;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Application.Json
{
    public interface IJSchemaManager
    {
        string[] GetObjectPropertyNames(JSchema jSchema);
        JSchema GetArrayItemSchema(JSchema jSchema);
        string[] GetArrayPropertyNames(JSchema jSchema);
        string GetArrayPropertyClassName(JSchema jSchema, string propertyName);
        string GetPropertyClassName(JSchema rootSchema, string propertyName);
        JSchema GetPropertySchema(JSchema rootSchema, string propertyName);
        string[] GetAllClassNames(JSchema jSchema);
        string GetObjectClassName(JSchema jSchema);
        string GetArrayClassName(JSchema jSchema);
        string[] GetPropertyNames(JSchema jSchema);
        string[] GetPropertyColumnNames(JSchema jSchema, string propertyName);
    }
}