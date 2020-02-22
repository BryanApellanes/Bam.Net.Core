using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bam.Net.Application.Json;
using GraphQL.Types;

namespace Bam.Net.Schema.Json
{
    public class JavaJSchemaManager : JSchemaManager
    {
        private List<string> _truncations;

        public JavaJSchemaManager() : base("@type", "title", "javaType")
        {            
            _truncations = new List<string>()
            {

                "Entity",
                "_v1",
                "_v1.yaml"
            };
            ParseClassNameFunction = cn =>
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
            ParsePropertyNameFunction = pn => pn.PascalCase();
            ExtractClassName = jSchema =>
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