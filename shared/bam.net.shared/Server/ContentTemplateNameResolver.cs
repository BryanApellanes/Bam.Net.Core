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

        public string ResolveTemplateName(ITemplateable templateable)
        {
            return templateable.TemplateName;
        }

        public string ResolveTemplateName(object toBeTemplated)
        {
            if(toBeTemplated == null)
            {
                return TemplateAttribute.DefaultTemplateName;
            }

            if (toBeTemplated is ITemplateable templateable)
            {
                return ResolveTemplateName(templateable);
            }
            Type type = toBeTemplated.GetType();
            if(type.HasCustomAttributeOfType(out TemplateAttribute attr))
            {
                return attr.Name;
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
