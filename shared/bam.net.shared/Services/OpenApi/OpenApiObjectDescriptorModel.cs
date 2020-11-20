using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.ServiceProxy;

namespace Bam.Net.Services.OpenApi
{
    public partial class OpenApiObjectDescriptorModel
    {
        public string Namespace { get; set; }
        public string ObjectName { get; set; }
        public string FormattedDescription => ObjectDescription.Replace("\n", "\n\t/// ");

        public string ObjectDescription { get; set; }

        public List<OpenApiFixedFieldModel> FixedFields { get; set; }

        public string Render()
        {
            return Bam.Net.Handlebars.Render("OpenApiObjectDescriptorModel", this);
        }
    }
}
