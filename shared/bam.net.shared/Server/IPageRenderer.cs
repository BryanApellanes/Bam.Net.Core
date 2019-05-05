using Bam.Net.ServiceProxy;

namespace Bam.Net.Server
{
    public interface IPageRenderer
    {
        bool CanRender(IRequest request);
        byte[] RenderPage(string path, IRequest request, IResponse response);
    }
}