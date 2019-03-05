using System.IO;

namespace Bam.Net
{
    public interface ITemplateRenderer
    {
        void Render(object toRender, Stream output);
        void Render(string templateName, object toRender, Stream output);
    }
}