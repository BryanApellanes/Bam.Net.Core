using System.ComponentModel;
using System.IO;
using System.Text;
using Bam.Net.Presentation;
using Bam.Net.ServiceProxy;

namespace Bam.Net.Server
{
    public abstract class AppPageRenderer : PageRenderer
    {
        public AppPageRenderer(AppContentResponder appContentResponder, ITemplateManager commonTemplateManager) : base(appContentResponder, commonTemplateManager)
        {
            InitializeDefaultRenderers();
        }

        public AppPageRenderer(AppContentResponder appContentResponder, ITemplateManager commonTemplateManager, IApplicationTemplateManager applicationTemplateManager) : base(appContentResponder, commonTemplateManager, applicationTemplateManager)
        {
            InitializeDefaultRenderers();
        }
        
        public IApplicationTemplateManager TemplateManager { get; set; }

        public Fs AppRoot => AppContentResponder.AppRoot;

        public AppConf AppConf => AppContentResponder.AppConf;

        public string FileExtension { get; protected set; }
        public int Precedence { get; set; }

        public string DefaultFilePath => Path.Combine("~/", AppConf.HtmlDir, $"{AppConf.DefaultPage}{FileExtension}");

        public override byte[] RenderDefault(int statusCode, IRequest request, IResponse response)
        {
            if (response.StatusCode != statusCode)
            {
                response.StatusCode = statusCode;
            }

            return DefaultRenderers.ContainsKey(statusCode) ? DefaultRenderers[statusCode](request, response) : new byte[]{};
        }

        protected virtual PageModel CreatePageModel(IRequest request)
        {
            return new PageModel(request, AppContentResponder);
        }
        
        /// <summary>
        /// Determines if a file exists for the specified request with the extension of the current renderer.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected internal virtual bool FileExists(IRequest request)
        {
            return GetRequestInfo(request).FileExists(AppConf);
        }

        protected virtual bool FileExists(IRequest request, out string absolutePath)
        {
            return GetRequestInfo(request).FileExists(AppConf, out absolutePath);
        }
        
        protected RequestInfo GetRequestInfo(IRequest request)
        {
            return new RequestInfo
            {
                RequestPath = request.Url.AbsolutePath,
                RelativePath = Path.Combine("~/", AppConf.HtmlDir, $"{request.Url.AbsolutePath}{FileExtension}"),
                RouteInfo =  GetRouteInfo(request)
            };
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
        
        protected byte[] RenderNotFound(IRequest request, IResponse response)
        {
            return RenderDefault(404, request, response);
        }
    }
}