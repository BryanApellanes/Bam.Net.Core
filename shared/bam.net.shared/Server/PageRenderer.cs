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
        public PageRenderer(AppContentResponder appContentResponder, ITemplateManager templateManager): base()
        {
            CommonTemplateManager = templateManager;
            ApplicationTemplateManager = new AppHandlebarsRenderer(appContentResponder);
            RequestRouter = new RequestRouter();
            AppContentResponder = appContentResponder;
        }

        public PageRenderer(AppContentResponder appContentResponder, ITemplateManager templateManager,
            IApplicationTemplateManager applicationTemplateManager)
        {
            CommonTemplateManager = templateManager;
            ApplicationTemplateManager = applicationTemplateManager;
            RequestRouter = new RequestRouter();
            ApplicationTemplateManager.AppContentResponder = appContentResponder;
            appContentResponder = appContentResponder;
        }
        
        protected RequestRouter RequestRouter { get; set; }

        public AppContentResponder AppContentResponder { get; set; }
        public IApplicationTemplateManager ApplicationTemplateManager { get; set; }
        public ITemplateManager CommonTemplateManager { get; set; }
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