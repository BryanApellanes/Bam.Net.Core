/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;
using Bam.Net;
using Bam.Net.ServiceProxy;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using System.IO;
using System.Text.Encodings.Web;

namespace Bam.Net.Presentation.Html
{

    public partial class Tag
    {
        public Tag(string tagName, object attributes = null, object content = null) : this(tagName,
            attributes?.ToDictionary(), content)
        {
        }

        public Tag(string tagName, Dictionary<string, object> attributes = null, object content = null)
        {
            TagName = tagName;
            Attributes = attributes ?? new Dictionary<string, object>();
            Content = content?.ToString();
            Styles = new Dictionary<string, object>();   
        }
        
        public string TagName { get; set; }
        public Dictionary<string, object> Attributes { get; private set; }
        public Dictionary<string, object> Styles { get; private set; }
        protected string Content { get; set; }

        public Tag AddAttributes(object attributes)
        {
            Args.ThrowIfNull(attributes, "attributes");
            foreach (KeyValuePair kvp in attributes.ToKeyValuePairs())
            {
                AddAttribute(kvp.Key, kvp.Value);
            }

            return this;
        }

        public Tag AddAttribute(string name, object value)
        {
            Attributes.AddMissing(name, value);
            return this;
        }

        public Tag SetAttribute(string name, object value)
        {
            if (!Attributes.AddMissing(name, value))
            {
                Attributes[name] = value;
            }

            return this;
        }
        
        public bool SelfClosing { get; set; }

        public static Tag Of(string tagName, object attributes = null, string content = null)
        {
            return new Tag(tagName, attributes, content);
        }
        
        public string Render()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(RenderStartOpenTag());
            if (Attributes != null)
            {
                stringBuilder.Append(RenderAttributes());
            }
            if (Content != null)
            {
                stringBuilder.Append(RenderEndOfTag());
                stringBuilder.Append(Content);
                stringBuilder.Append(RenderEndTag());
            }
            else
            {
                if (SelfClosing)
                {
                    stringBuilder.Append(RenderEndOfSelfClosingTag());
                }
                else
                {
                    stringBuilder.Append(RenderEndOfTag());
                    stringBuilder.Append(RenderEndTag());
                }
            }

            return stringBuilder.ToString();
        }

        private string RenderStartOpenTag()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"<{TagName}");
            
            return stringBuilder.ToString();
        }

        private string RenderEndTag()
        {
            return $"</{TagName}>";
        }

        private string RenderEndOfSelfClosingTag()
        {
            return "/>";
        }

        private string RenderEndOfTag()
        {
            return ">";
        }

        private string RenderAttributes()
        {
            Type type = Attributes.GetType();
            PropertyInfo[] props = type.GetProperties();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" ");
            for (int i = 0; i < props.Length; i++)
            {
                PropertyInfo prop = props[i];
                stringBuilder.Append($"{prop.Name}=\"{prop.GetValue(Attributes)}\"");
                if (i != props.Length - 1)
                {
                    stringBuilder.Append(" ");
                }
            }

            return stringBuilder.ToString();
        }
    }
}
