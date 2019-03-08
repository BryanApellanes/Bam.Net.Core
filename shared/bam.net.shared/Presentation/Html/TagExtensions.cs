using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bam.Net.Presentation.Html
{
    public static class TagExtensions
    {
        public static string Render(object element)
        {
            Type type = element.GetType();
            string tagName = "div";
            string content = "&nbsp;";
            Dictionary<string, object> attributes = new Dictionary<string, object>();
            foreach (PropertyInfo prop in type.GetProperties())
            {
                if (prop.Name.Equals("TagName"))
                {
                    tagName = prop.GetValue(element).Cast<string>();
                }else if (prop.Name.Equals("Content"))
                {
                    content = prop.GetValue(element).Cast<string>();
                }
                else
                {
                    attributes.Add(prop.Name, prop.GetValue(element));
                }
            }
            // TODO: finish this
            //return new Tag(tagName, );
        }
    }
}