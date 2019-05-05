using System.Collections.Generic;
using Bam.Net.Logging;
using Bam.Net.Presentation;
using Bam.Net.Server.Renderers;
using Bam.Net.ServiceProxy;
using Bam.Net.Services;

namespace Bam.Net.Server
{
    // TODO: implement this
    // 
    public class BamPageRenderer : Loggable, IPageRenderer
    {
        public BamPageRenderer(): base()
        {
            TemplateManager = new AppHandlebarsRenderer();
            RequestRouter = new RequestRouter();
            _renderedPages = new Dictionary<string, byte[]>();
        }

        public BamPageRenderer(IApplicationTemplateManager templateManager)
        {
            TemplateManager = templateManager;
            RequestRouter = new RequestRouter();
            _renderedPages = new Dictionary<string, byte[]>();
        }
        
        protected RequestRouter RequestRouter { get; set; }

        public IApplicationTemplateManager TemplateManager { get; set; }

        public bool CanRender(IRequest request)
        {
            bool isHomeRequest = RequestRouter.IsHomeRequest(request.RawUrl, out RequestRoute route);
            if (!isHomeRequest)
            {
                return route.IsValid;
            }

            return isHomeRequest;
        }

        Dictionary<string, byte[]> _renderedPages;
        
        public byte[] RenderPage(string path, IRequest request, IResponse response)
        {
            throw new System.NotImplementedException();
        }
    }
}