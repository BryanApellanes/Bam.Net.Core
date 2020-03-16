using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bam.Net.Application.Json;
using GraphQL.Types;

namespace Bam.Net.Schema.Json
{
    public class JavaJSchemaClassManager : JSchemaClassManager
    {
        private List<string> _truncations;

        public JavaJSchemaClassManager() : base("@type", "javaType", "class", "className")
        {            
            SetClassNameMunger("javaType", javaType =>
            {
                string[] split = javaType.DelimitSplit(".");
                string typeName = split[split.Length - 1];
                if (typeName.EndsWith("Entity"))
                {
                    typeName = typeName.Truncate("Entity".Length);
                }

                return typeName;
            });
        }
    }
}