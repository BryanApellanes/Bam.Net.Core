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
            _routeInfos = new Dictionary<string, RouteInfo>();
        }

        public PageRenderer(AppContentResponder appContentResponder, ITemplateManager templateManager,
            IApplicationTemplateManager applicationTemplateManager)
        {
            CommonTemplateManager = templateManager;
            ApplicationTemplateManager = applicationTemplateManager;
            RequestRouter = new RequestRouter();
            ApplicationTemplateManager.AppContentResponder = appContentResponder;
            AppContentResponder = appContentResponder;
            _routeInfos = new Dictionary<string, RouteInfo>();
        }
        
        protected RequestRouter RequestRouter { get; set; }

        public AppContentResponder AppContentResponder { get; set; }
        public IApplicationTemplateManager ApplicationTemplateManager { get; set; }
        public ITemplateManager CommonTemplateManager { get; set; }
        public bool CanRender(IRequest request)
        {
            return GetRouteInfo(request).CanRender;
        }

        public abstract byte[] RenderPage(IRequest request, IResponse response);

        Dictionary<string, RouteInfo> _routeInfos;

        protected RouteInfo GetRouteInfo(IRequest request)
        {
            string url = request.Url?.ToString();
            if (!_routeInfos.ContainsKey(url))
            {
                RouteInfo info = new RouteInfo() {Request = request};
                info.IsHomeRequest = RequestRouter.IsHomeRequest(request.Url, out RequestRoute route);
                info.RequestRoute = route;
                if (!info.IsHomeRequest)
                {
                    info.CanRender = route.IsValid;
                }
                else
                {
                    info.CanRender = info.IsHomeRequest;
                }

                _routeInfos.AddMissing(url, info);
            }
            
            return _routeInfos[url];
        }
    }
}