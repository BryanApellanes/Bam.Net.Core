using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.Data.Repositories
{
    /// <summary>
    /// Used to annotate a property that, together with other properties, uniquely identifies
    /// an instance.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CompositeKeyAttribute : Attribute { }
}
