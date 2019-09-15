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
            InitializeDefaultRenderers();
        }

        public AppPageRenderer(AppContentResponder appContentResponder, ITemplateManager commonTemplateManager,
            IApplicationTemplateManager applicationTemplateManager) : base(appContentResponder, commonTemplateManager, applicationTemplateManager)
        {
            InitializeDefaultRenderers();
        }
        
        public IApplicationTemplateManager TemplateManager { get; set; }

        public Fs AppRoot => AppContentResponder.AppRoot;

        public AppConf AppConf => AppContentResponder.AppConf;

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

            return RenderNotFound(request, response);
        }

        public override byte[] RenderDefault(int statusCode, IRequest request, IResponse response)
        {
            if (response.StatusCode != statusCode)
            {
                response.StatusCode = statusCode;
            }

            return DefaultRenderers.ContainsKey(statusCode) ? DefaultRenderers[statusCode](request, response) : new byte[]{};
        }

        protected virtual void InitializeDefaultRenderers()
        {
            DefaultRenderers.Add(404, (request, response) =>
            {
                string notFoundHtml = $@"<!DOCTYPE html>
<html>
<body>
<h2>404 Page Not Found: {request.Url}</h2>
</body>
</html>";
                response.StatusCode = 404;
                return Encoding.UTF8.GetBytes(notFoundHtml);
            });
        }
        
        private byte[] RenderNotFound(IRequest request, IResponse response)
        {
            return RenderDefault(404, request, response);
        }
    }
}