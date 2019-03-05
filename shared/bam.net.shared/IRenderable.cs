using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Bam.Net
{
    public interface IRenderable
    {
        string Render();
        void Render(Stream output);
        void Render(ITemplateRenderer renderer);
        void Render(ITemplateRenderer renderer, string templateName, Stream output);
    }
}
