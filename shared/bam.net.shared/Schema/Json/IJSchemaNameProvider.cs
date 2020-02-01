using System.Collections.Generic;
using Bam.Net.Data.Schema;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Application.Json
{
    public interface IJSchemaNameProvider
    {
        string GetRootTableName(JSchema jSchema);
        string GetRootClassName(JSchema jSchema);
        string[] GetColumnNames(JSchema jSchema);
        SchemaNameMap GetSchemaNameMap(JSchema jSchema);
    }
}