using System;
using System.Linq;

namespace Bam.Net.Data.Repositories
{
    public class TypeSchemaException: Exception
    {
        public TypeSchemaException(params TypeSchemaWarning[] warnings) : base(string.Join("\r\n", warnings.Select(w => w.ToString())))
        {
        }
    }
}