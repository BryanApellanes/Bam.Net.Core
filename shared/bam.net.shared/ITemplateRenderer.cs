using System.IO;

namespace Bam.Net
{
    public interface ITemplateRenderer
    {
        string Render(string templateName, object toRender);
        void Render(object toRender, Stream output);
        void Render(string templateName, object toRender, Stream output);
    }
}