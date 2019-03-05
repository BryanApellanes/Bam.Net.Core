using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Presentation
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TemplateNameAttribute: Attribute
    {
        public TemplateNameAttribute()
        {
            TemplateName = DefaultTemplateName;
        }
        public static string DefaultTemplateName { get { return "Default"; } }

        public string TemplateName { get; set; }
    }
}
