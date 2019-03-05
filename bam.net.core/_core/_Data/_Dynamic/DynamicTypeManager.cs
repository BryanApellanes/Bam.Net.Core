using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.Data.Dynamic
{
    public partial class DynamicTypeManager
    {
        protected void SetReferenceAssemblies()
        {
            List<Assembly> assemblies = new List<Assembly>(Compiler.ReferenceAssemblies)
            {
                typeof(ActionResult).Assembly
            };
            Compiler.ReferenceAssemblies = assemblies.ToArray();
        }
    }
}
