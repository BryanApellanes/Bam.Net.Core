using Bam.Net.Presentation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bam.Net.Application
{
    public abstract class RenderableModel : IRenderable, IRenderer
    {
        public ITemplateRenderer TemplateRenderer { get; set; }
        public Stream Output { get; set; }

        public virtual string Render()
        {
            return Render(this);
        }

        readonly object _renderLock = new object();
        public void Render(Stream output)
        {
            Args.ThrowIfNull(TemplateRenderer);
            lock (_renderLock)
            {
                TemplateRenderer.Render(this, output);
                Output = output;
            }
        }

        public void Render(ITemplateRenderer renderer)
        {
            Args.ThrowIfNull(renderer, "renderer");
            lock (_renderLock)
            {
                MemoryStream stream = new MemoryStream();
                renderer.Render(this, stream);
                TemplateRenderer = renderer;
                Output = stream;
            }
        }

        public void Render(ITemplateRenderer renderer, string templateName, Stream output)
        {
            Args.ThrowIfNull(renderer, "renderer");
            Args.ThrowIfNullOrEmpty(templateName, "templateName");
            Args.ThrowIfNull(output, "output");
            lock (_renderLock)
            {
                renderer.Render(templateName, this, output);
                TemplateRenderer = renderer;
                Output = output;
            }
        }

        public string Render(object toRender)
        {
            Args.ThrowIfNull(TemplateRenderer);
            Args.ThrowIfNull(toRender, "toRender");

            MemoryStream memoryStream = new MemoryStream();
            TemplateRenderer.Render(toRender, memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            Output = memoryStream;
            return memoryStream.ReadToEnd();
        }
    }
}
