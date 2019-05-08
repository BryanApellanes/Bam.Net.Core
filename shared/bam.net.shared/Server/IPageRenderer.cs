using Bam.Net.Presentation;
using Bam.Net.ServiceProxy;

namespace Bam.Net.Server
{
    public interface IPageRenderer
    {
        IApplicationTemplateManager ApplicationTemplateManager { get; set; }
        bool CanRender(IRequest request);
        byte[] RenderPage(string path, IRequest request, IResponse response);
    }
}