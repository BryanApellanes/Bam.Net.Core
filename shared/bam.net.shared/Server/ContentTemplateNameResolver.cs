using Bam.Net.Presentation;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Bam.Net.Server
{
    public class ContentTemplateNameResolver : ITemplateNameResolver
    {
        public ContentTemplateNameResolver(ContentResponder content)
        {
            ContentResponder = content;
        }
        
        public ContentResponder ContentResponder
        {
            get;
        }

        public string ResolveTemplateName(object toBeTemplated)
        {
            if(toBeTemplated == null)
            {
                return TemplateNameAttribute.DefaultTemplateName;
            }
            Type type = toBeTemplated.GetType();
            if(type.HasCustomAttributeOfType(out TemplateNameAttribute attr))
            {
                return attr.TemplateName;
            }
            if(toBeTemplated.HasProperty("View", out PropertyInfo prop))
            {
                return prop.GetValue(toBeTemplated)?.ToString();
            }
            else if(toBeTemplated.HasProperty("Template", out PropertyInfo prop2))
            {
                return prop2.GetValue(toBeTemplated)?.ToString();
            }
            else if(toBeTemplated.HasProperty("TemplateName", out PropertyInfo prop3))
            {
                return prop3.GetValue(toBeTemplated)?.ToString();
            }
            else if(toBeTemplated.HasProperty("LayoutName", out PropertyInfo prop4))
            {
                return prop4.GetValue(toBeTemplated)?.ToString();
            }
            return $"{type.Namespace}.{type.Name}";
        }
    }
}
