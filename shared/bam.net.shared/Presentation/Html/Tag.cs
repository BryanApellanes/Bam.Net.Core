using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Presentation.Html
{
    public static class Tag
    {
        public static HtmlTag Of(string name, object attributes = null)
        {
            HtmlTag result = new HtmlTag { Name = name };
            if(attributes != null)
            {
                result.Attributes = attributes.ToDictionary(o => o?.ToString());
            }
            return result;
        }
    }
}
