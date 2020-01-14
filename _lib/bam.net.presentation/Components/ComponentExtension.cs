using Bam.Net.Data;
using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;
using UnityEngine;

namespace Bam.Net.Presentation.Components
{
    public class ComponentExtension : IMarkdownExtension
    {
        private readonly ComponentOptions _options;
        public ComponentExtension(ComponentOptions options)
        {
            _options = options;
        }
        
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            OrderedList<InlineParser> parsers = pipeline.InlineParsers;
            if (!parsers.Contains<ComponentParser>())
            {
                parsers.Add(new ComponentParser());
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            HtmlRenderer htmlRenderer = renderer as HtmlRenderer;
            ObjectRendererCollection renderers = htmlRenderer?.ObjectRenderers;

            if (renderers != null && !renderers.Contains<ComponentRenderer>())
            {
                renderers.Add(new ComponentRenderer());
            }
        }
    }

    public static class BamComponentExtensions
    {
        public static MarkdownPipelineBuilder UseBamComponents(this MarkdownPipelineBuilder pipeline, ComponentOptions options)
        {
            OrderedList<IMarkdownExtension> extensions = pipeline.Extensions;
            if (!extensions.Contains<ComponentExtension>())
            {
                extensions.Add(new ComponentExtension(options));
            }

            return pipeline;
        }
    }
}