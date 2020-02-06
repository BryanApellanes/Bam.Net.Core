using System;
using System.IO;
using Bam.Net.Presentation.Handlebars;
using Bam.Net.Server;
using Bam.Net.ServiceProxy;

namespace Bam.Net.Presentation.AppRenderers
{
    public class ViewModelAppPageRenderer : HandlebarsAppPageRenderer
    {
        public ViewModelAppPageRenderer(AppContentResponder appContentResponder, ITemplateManager commonTemplateManager) : base(appContentResponder, commonTemplateManager)
        {
            this.FileExtension = ".bvm"; // bam view model
        }

        public ViewModelAppPageRenderer(AppContentResponder appContentResponder, ITemplateManager commonTemplateManager, IApplicationTemplateManager applicationTemplateManager) : base(appContentResponder, commonTemplateManager, applicationTemplateManager)
        {
            this.FileExtension = ".bvm"; // bam view model
        }

        public override byte[] RenderPage(IRequest request, IResponse response)
        {
            throw new System.NotImplementedException();
        }

        protected override PageModel CreatePageModel(IRequest request)
        {
            if (FileExists(request, out string absolutePath))
            {
                FileInfo fileInfo = new FileInfo(absolutePath);
                ViewModelFile viewModelFile = new ViewModelFile(){Path = fileInfo.FullName};
                return new PageModel(request, AppContentResponder)
                {
                    ViewModel =  viewModelFile.Load(AppConf)
                };
            }
            
            throw new NotImplementedException();
        }
    }
}