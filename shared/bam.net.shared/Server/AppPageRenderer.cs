using System.ComponentModel;
using System.IO;
using System.Text;
using Bam.Net.Presentation;
using Bam.Net.ServiceProxy;
using Lucene.Net.Support;

namespace Bam.Net.Server
{
    public class AppPageRenderer : PageRenderer
    {
        public AppPageRenderer(AppContentResponder appContentResponder, ITemplateManager commonTemplateManager) : base(appContentResponder, commonTemplateManager)
        {
        }

        public AppPageRenderer(AppContentResponder appContentResponder, ITemplateManager commonTemplateManager,
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
            string relativePath = Path.Combine("~/", AppConf.HtmlDir, $"{AppConf.DefaultPage}.html");
            RouteInfo routeInfo = GetRouteInfo(request);
            if (routeInfo.IsHomeRequest)
            {
                if (AppRoot.FileExists(relativePath, out string locatedPath))
                {
                    return AppContentResponder.GetContent(locatedPath, request, response);
                }
            }
            else 
            {
                string absolutePath = AppRoot.GetAbsolutePath(relativePath);
                if (File.Exists(absolutePath))
                {
                    return AppContentResponder.GetContent(absolutePath, request, response);
                }
            }

            string notFoundHtml = $@"<!DOCTYPE html>
<html>
<body>
<h2>404 Not Found: {request.Url}</h2>
</body>
</html>";
            return Encoding.UTF8.GetBytes(notFoundHtml);
        }
    }
}