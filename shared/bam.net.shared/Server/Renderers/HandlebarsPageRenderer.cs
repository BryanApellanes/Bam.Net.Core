using System.Reflection;
using Bam.Net.Presentation;
using Bam.Net.ServiceProxy;

namespace Bam.Net.Server.Renderers
{
    public class HandlebarsPageRenderer : FileExtensionPageRenderer
    {
        public HandlebarsPageRenderer(AppContentResponder appContentResponder, ITemplateManager templateManager) : base(appContentResponder, templateManager)
        {
            Extension = ".hbs";
        }

        public HandlebarsPageRenderer(AppContentResponder appContentResponder, ITemplateManager templateManager, IApplicationTemplateManager applicationTemplateManager) : base(appContentResponder, templateManager, applicationTemplateManager)
        {
            Extension = ".hbs";
        }

        public override byte[] RenderPage(IRequest request, IResponse response)
        {
            string path = request.Url.AbsolutePath;
            throw new System.NotImplementedException();
        }

        public override byte[] RenderDefault(int statusCode, IRequest request, IResponse response)
        {
            throw new System.NotImplementedException();
        }
    }
}