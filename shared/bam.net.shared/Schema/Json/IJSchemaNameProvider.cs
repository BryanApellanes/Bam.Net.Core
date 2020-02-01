using System.Collections.Generic;
using Bam.Net.Data.Schema;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Application.Json
{
    public interface IJSchemaNameProvider
    {
        string[] GetObjectPropertyNames(JSchema jSchema);
        JSchema GetPropertySchema(JSchema rootSchema, string propertyName);
        string[] GetAllTableNames(JSchema jSchema);
        string GetTableName(JSchema jSchema);
        string GetClassName(JSchema jSchema);
        string[] GetColumnNames(JSchema jSchema);
        string[] GetPropertyColumnNames(JSchema jSchema, string propertyName);
    }
}