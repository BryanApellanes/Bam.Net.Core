using Bam.Net.Presentation.Html;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Presentation.Components
{
    public class ComponentRenderer<T> : HtmlObjectRenderer<T> where T : MarkdownObject
    {
        public ComponentRenderer()
        {
            InputFormBuilder = new InputFormBuilder();
        }

        public InputFormBuilder InputFormBuilder { get; set; }

        protected override void Write(HtmlRenderer renderer, T obj)
        {
            renderer.Write(InputFormBuilder.FieldsetFor(obj).Render());
        }
    }
}
