using System;
using System.IO;
using Bam.Net.Presentation.Handlebars;
using Bam.Net.Server;
using Bam.Net.ServiceProxy;

namespace Bam.Net.Presentation.AppRenderers
{
    public class ViewModelRenderer : HandlebarsAppPageRenderer
    {
        public ViewModelRenderer(AppContentResponder appContentResponder, ITemplateManager commonTemplateManager) : base(appContentResponder, commonTemplateManager)
        {
            this.FileExtension = ".bvm"; // bam view model
            this.Precedence = 100;
        }

        public ViewModelRenderer(AppContentResponder appContentResponder, ITemplateManager commonTemplateManager, IApplicationTemplateManager applicationTemplateManager) : base(appContentResponder, commonTemplateManager, applicationTemplateManager)
        {
            this.FileExtension = ".bvm"; // bam view model
            this.Precedence = 100;
        }

        protected ContentLocator ContentLocator => AppContentResponder.AppContentLocator;

        protected internal override bool FileExists(IRequest request)
        {
            return FileExists(request, out string ignore);
        }
        
        protected override bool FileExists(IRequest request, out string absolutePath)
        {
            if (base.FileExists(request, out absolutePath))
            {
                return true;
            }

            string path = request.Url.AbsolutePath.Equals("/") ? DefaultFilePath : request.Url.AbsolutePath; 
            string fileName = Path.GetFileName(path);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string myFileName = $"{fileNameWithoutExtension}{FileExtension}";
            return ContentLocator.Locate(myFileName, out absolutePath, out string[] checkedPaths);
        }

        protected override PageModel CreatePageModel(IRequest request)
        {
            if (FileExists(request, out string absolutePath))
            {
                FileInfo fileInfo = new FileInfo(absolutePath);
                ViewModelFile viewModelFile = new ViewModelFile {Path = fileInfo.FullName};
                ViewModel viewModel = viewModelFile.Load(AppConf);
                viewModel.ViewModelId = viewModelFile.ViewModelId;
                
                TemplateRenderer.AddTemplate(viewModelFile.ViewModelId, viewModelFile.GetSource());
                
                return new PageModel(request, AppContentResponder)
                {
                    ViewModel =  viewModel
                };
            }

            return base.CreatePageModel(request);
        }

        protected override string GetTemplateName(IRequest request)
        {
            if (FileExists(request, out string absolutePath))
            {
                FileInfo fileInfo = new FileInfo(absolutePath);
                ViewModelFile viewModelFile = new ViewModelFile {Path = fileInfo.FullName};
                viewModelFile.ParseActionProviderName(AppConf);
                return viewModelFile.ViewModelId;
            }
            return base.GetTemplateName(request);
        }
    }
}