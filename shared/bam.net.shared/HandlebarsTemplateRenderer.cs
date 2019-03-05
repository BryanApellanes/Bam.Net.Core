using Bam.Net.Presentation.Handlebars;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bam.Net
{
    public class HandlebarsTemplateRenderer : ITemplateRenderer
    {
        public HandlebarsTemplateRenderer(HandlebarsDirectory handlebarsDirectory, HandlebarsEmbeddedResources handlebarsEmbeddedResources)
        {
            HandlebarsDirectory = handlebarsDirectory;
            HandlebarsEmbeddedResources = handlebarsEmbeddedResources;

            HandlebarsDirectory.Reload();
            HandlebarsEmbeddedResources.Reload();
        }

        public HandlebarsDirectory HandlebarsDirectory { get; set; }
        public HandlebarsEmbeddedResources HandlebarsEmbeddedResources { get; set; }

        public void Render(object toRender, Stream output)
        {
            Args.ThrowIfNull(toRender, "toRender");
            Render(toRender.GetType().Name, toRender, output);
        }

        public void Render(string templateName, object renderModel, Stream output)
        {
            HandlebarsDirectory = HandlebarsDirectory ?? Handlebars.HandlebarsDirectory;
            HandlebarsEmbeddedResources = HandlebarsEmbeddedResources ?? Handlebars.HandlebarsEmbeddedResources;
            Args.ThrowIfNull(HandlebarsDirectory, "HandlebarsDirectory");
            Args.ThrowIfNull(HandlebarsEmbeddedResources, "HandlebarsEmbeddedResources");
            
            if ((HandlebarsDirectory?.Templates?.ContainsKey(templateName)) == true)
            {
                string code = HandlebarsDirectory.Render(templateName, renderModel);

                code.WriteToStream(output);
            }
            else if ((HandlebarsEmbeddedResources?.Templates?.ContainsKey(templateName)) == true)
            {
                string code = HandlebarsEmbeddedResources.Render(templateName, renderModel);
                code.WriteToStream(output);
            }
            else
            {
                Args.Throw<InvalidOperationException>("Specified template {0} not found", templateName);
            }
        }
    }
}
