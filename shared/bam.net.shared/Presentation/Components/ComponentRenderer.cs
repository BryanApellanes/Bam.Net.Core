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
            InputFormProviderBuilder = new InputFormProvider();
        }

        public InputFormProvider InputFormProviderBuilder { get; set; }

        protected override void Write(HtmlRenderer renderer, T obj)
        {
            renderer.Write(InputFormProviderBuilder.FieldsetFor(obj).Render());
        }
    }
}
