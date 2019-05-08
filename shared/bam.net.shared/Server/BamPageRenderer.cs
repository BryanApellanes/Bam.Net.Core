using System.IO;
using Bam.Net.Presentation;
using Bam.Net.ServiceProxy;

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

        public override byte[] RenderPage(string path, IRequest request, IResponse response)
        {
            throw new System.NotImplementedException();
        }
    }
}