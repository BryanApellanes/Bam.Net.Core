using Bam.Net.ServiceProxy;

namespace Bam.Net.Server
{
    // TODO: implement this
    // 
    public class BamPageRenderer : IPageRenderer
    {
        public bool CanRender(IRequest request)
        {
            return false;
        }

        public byte[] RenderPage(string path, IRequest request, IResponse response)
        {
            throw new System.NotImplementedException();
        }
    }
}