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

        public JavaJSchemaClassManager() : base("@type", "title", "javaType")
        {            
            _truncations = new List<string>()
            {

                "Entity",
                "_v1",
                "_v1.yaml"
            };
            MungeClassName = cn =>
            {
                string className = cn.DelimitSplit(".").Last();
                foreach (string truncation in _truncations)
                {
                    if (className.EndsWith(truncation))
                    {
                        className = className.Truncate(truncation.Length);
                    }
                }

                return className;
            };
            ParsePropertyName = pn => pn.PascalCase();
            ExtractJSchemaClassName = jSchema =>
            {
                if (!string.IsNullOrEmpty(jSchema?.Title))
                {
                    FileInfo fi = new FileInfo(jSchema.Title);
                    return Path.GetFileNameWithoutExtension(fi.Name).PascalCase();
                }

                return string.Empty;
            };
        }
    }
}