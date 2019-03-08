/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;
using Bam.Net.ServiceProxy;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using System.IO;
using System.Text.Encodings.Web;

namespace Bam.Net.Presentation.Html
{

    public partial class Tag
    {
        public Tag(string tagName, object attributes = null, string content = null)
        {
            TagName = tagName;
            Attributes = attributes;
            Content = content;
        }
        public string TagName { get; set; }
        protected object Attributes { get; set; }
        protected string Content { get; set; }

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
