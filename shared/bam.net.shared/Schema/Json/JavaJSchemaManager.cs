using System.Linq;
using Bam.Net.Application.Json;

namespace Bam.Net.Schema.Json
{
    public class JavaJSchemaManager : JSchemaManager
    {
        public JavaJSchemaManager() : base("@type", "title", "javaType")
        {
            ParseClassNameFunction = cn =>
            {
                string tableName = cn.DelimitSplit(".").Last();
                if (tableName.EndsWith("Entity"))
                {
                    tableName = tableName.Truncate("Entity".Length);
                }

                return tableName;
            };
            ParsePropertyNameFunction = pn => pn.PascalCase();
        }
    }
}