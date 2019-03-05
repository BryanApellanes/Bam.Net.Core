using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.ServiceProxy;
using Newtonsoft.Json;

namespace Bam.Net.Services.OpenApi
{
    public partial class OpenApiFixedFieldModel
    {
        public string FieldName { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public string ClrType
        {
            get
            {
                if (Type.Trim().StartsWith("Map", StringComparison.InvariantCultureIgnoreCase))
                {
                    return "Dictionary<string, object>";
                }
                else if (Type.Trim().StartsWith("Any"))
                {
                    return "object";
                }
                else
                {
                    string[] split = Type.DelimitSplit("[", "]", "<", ">");
                    string clrType = split.Length > 1 ? split[1].PascalCase() : split[0];
                    if (clrType.Equals("boolean"))
                    {
                        return "bool";
                    }
                    return clrType;
                }
            }
        }

        public string PropertyName
        {
            get
            {
                return FieldName.Replace("$", "").PascalCase(true, " ");
            }
        }
    }
}
