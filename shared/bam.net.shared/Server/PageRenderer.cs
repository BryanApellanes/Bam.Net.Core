using System.Collections.Generic;
using Bam.Net.Logging;
using Bam.Net.Presentation;
using Bam.Net.Server.Renderers;
using Bam.Net.ServiceProxy;
using Bam.Net.Services;

namespace Bam.Net.Server
{
    public abstract class PageRenderer : Loggable, IPageRenderer
    {
        public PageRenderer(): base()
        {
            TemplateManager = new AppHandlebarsRenderer();
            RequestRouter = new RequestRouter();
        }

        public PageRenderer(IApplicationTemplateManager templateManager)
        {
            TemplateManager = templateManager;
            RequestRouter = new RequestRouter();
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

        public abstract byte[] RenderPage(string path, IRequest request, IResponse response);
    }
}