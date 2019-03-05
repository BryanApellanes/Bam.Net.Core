using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Web;

namespace Bam.Net.Presentation.Html
{
    public class HtmlTag
    {
        public string Name { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
        public string InnerText { get; set; }
        public string InnerHtml { get; set; }

        public bool SelfClosing { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"<{Name} {GetAttributes()}");
            if (!string.IsNullOrEmpty(InnerText))
            {
                stringBuilder.Append(">");
                stringBuilder.Append(HttpUtility.HtmlEncode(InnerText));
                stringBuilder.Append($"</ {Name}>");
            }
            else if (!string.IsNullOrEmpty(InnerHtml))
            {
                stringBuilder.Append(">");
                stringBuilder.Append(InnerHtml);
                stringBuilder.Append($"</ {Name}>");
            }
            else if (SelfClosing)
            {
                stringBuilder.Append(" />");
            }
            else
            {
                stringBuilder.Append($"></ {Name}>");
            }
            return stringBuilder.ToString();
        }

        private string GetAttributes()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach(string key in Attributes.Keys)
            {
                stringBuilder.AppendFormat("{0}=\"{1}\"", key, Attributes[key]);
            }
            return stringBuilder.ToString();
        }
    }
}
