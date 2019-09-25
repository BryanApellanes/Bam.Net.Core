using Bam.Net.Presentation;
using Bam.Net.ServiceProxy;

namespace Bam.Net.Server.Renderers
{
    public abstract class FileExtensionPageRenderer : PageRenderer
    {
        public FileExtensionPageRenderer(AppContentResponder appContentResponder, ITemplateManager templateManager) : base(appContentResponder, templateManager)
        {
        }

        public FileExtensionPageRenderer(AppContentResponder appContentResponder, ITemplateManager templateManager, IApplicationTemplateManager applicationTemplateManager) : base(appContentResponder, templateManager, applicationTemplateManager)
        {
        }
        
        public string Extension { get; protected set; }
    }
}