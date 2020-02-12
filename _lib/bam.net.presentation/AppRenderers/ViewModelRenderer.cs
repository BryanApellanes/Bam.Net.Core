using System;
using System.Collections.Generic;
using System.IO;
using Bam.Net.Presentation.Handlebars;
using Bam.Net.Server;
using Bam.Net.ServiceProxy;

namespace Bam.Net.Presentation.AppRenderers
{
    public class ViewModelRenderer : HandlebarsAppPageRenderer // TODO: consider extending AppPageRenderer instead of HandlebarsAppPageRenderer since all methods there are overridden
    {
        public ViewModelRenderer(AppContentResponder appContentResponder, ITemplateManager commonTemplateManager) : base(appContentResponder, commonTemplateManager)
        {
            this.FileExtension = ".bvm"; // bam view model
            this.Precedence = 100;
            this.Templates = new Dictionary<string, Func<object, string>>();
        }

        public ViewModelRenderer(AppContentResponder appContentResponder, ITemplateManager commonTemplateManager, IApplicationTemplateManager applicationTemplateManager) : base(appContentResponder, commonTemplateManager, applicationTemplateManager)
        {
            this.FileExtension = ".bvm"; // bam view model
            this.Precedence = 100;
            this.Templates = new Dictionary<string, Func<object, string>>();
        }
        
        public Dictionary<string, Func<object, string>> Templates { get; private set; }
        
        protected ContentLocator ContentLocator => AppContentResponder.AppContentLocator;

        public override byte[] RenderPage(IRequest request, IResponse response)
        {
            string templateName = GetTemplateName(request);
            PageModel pageModel = CreatePageModel(request);

            if (!string.IsNullOrEmpty(templateName) && Templates.ContainsKey(templateName))
            {
                return Templates[templateName](pageModel.TemplateData()).ToBytes();
            }
            
            return new byte[] { };
        }

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
                ViewModelTemplate viewModelFile = new ViewModelTemplate {FileSystemPath = fileInfo.FullName};
                string relativeFilePath = $"{fileInfo.FullName.TruncateFront(AppRoot.Root.Length)}";
                ViewModel viewModel = viewModelFile.Load(AppConf);
                
                PageModel pageModel = new PageModel(request, AppContentResponder)
                {
                    ViewModel =  viewModel
                };
                viewModel.Url.RelativeFilePath = relativeFilePath;
                AddCompiledTemplateFile(GetTemplateName(request), viewModelFile);

                return pageModel;
            }

            return base.CreatePageModel(request);
        }
        
        public void AddCompiledTemplateFile(string templateName, ViewModelTemplate file)
        {
            string content = file.GetSource();
            Func<object, string> template = HandlebarsDotNet.Handlebars.Compile(content);
            Templates.AddMissing(templateName, template);
        }
        
        /// <summary>
        /// This implementation returns the view model file id as the template name.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override string GetTemplateName(IRequest request)
        {
            if (FileExists(request, out string absolutePath))
            {
                FileInfo fileInfo = new FileInfo(absolutePath);
                ViewModelTemplate viewModelFile = new ViewModelTemplate
                {
                    FileSystemPath = fileInfo.FullName
                };
                viewModelFile.ParseActionProviderName(AppConf);
                return viewModelFile.ViewModelId;
            }
            return base.GetTemplateName(request);
        }
    }
}