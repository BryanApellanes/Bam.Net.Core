using System.ComponentModel;
using System.IO;
using System.Text;
using Bam.Net.Presentation;
using Bam.Net.ServiceProxy;
using Lucene.Net.Support;

namespace Bam.Net.Server
{
    public class BamPageRenderer : PageRenderer
    {
        public BamPageRenderer(AppContentResponder appContentResponder, ITemplateManager commonTemplateManager) : base(appContentResponder, commonTemplateManager)
        {
        }

        public BamPageRenderer(AppContentResponder appContentResponder, ITemplateManager commonTemplateManager,
            IApplicationTemplateManager applicationTemplateManager) : base(appContentResponder, commonTemplateManager, applicationTemplateManager)
        {
        }
        
        public IApplicationTemplateManager TemplateManager { get; set; }

        public Fs AppRoot
        {
            get { return AppContentResponder.AppRoot; }
        }

        public AppConf AppConf
        {
            get { return AppContentResponder.AppConf; }
        }

        public override byte[] RenderPage(IRequest request, IResponse response)
        {
            string path = request.Url.AbsolutePath;
            RouteInfo routeInfo = GetRouteInfo(request);
            if (routeInfo.IsHomeRequest)
            {
                string relativePath = Path.Combine("~/", AppConf.HtmlDir, $"{AppConf.DefaultPage}.html");
                if (AppRoot.FileExists(relativePath, out string locatedPath))
                {
                    return AppContentResponder.GetContent(locatedPath, request, response);
                }
            }
            else 
            {
                string absolutePath = AppRoot.GetAbsolutePath($"~/{AppConf.HtmlDir}{path}.html");
                if (File.Exists(absolutePath))
                {
                    return AppContentResponder.GetContent(absolutePath, request, response);
                }
            }
            
            return Encoding.UTF8.GetBytes($"<h2>404 Not Found: {request.RawUrl}</h2>");
        }
    }
}