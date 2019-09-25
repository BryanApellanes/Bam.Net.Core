using Bam.Net.Presentation;
using Bam.Net.Presentation.Handlebars;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bam.Net.Server.Renderers
{
    public class AppHandlebarsRenderer : CommonHandlebarsRenderer, IApplicationTemplateManager
    {
        public AppHandlebarsRenderer(AppContentResponder appContent) : base(appContent.ContentResponder)
        {
            AppContentResponder = appContent;
        }

        AppContentResponder _appContentResponder;
        public AppContentResponder AppContentResponder
        {
            get
            {
                return _appContentResponder;
            }
            set
            {
                _appContentResponder = value;
                SetHandlebarsDirectories();
            }
        }

        ContentResponder _contentResponder;
        public override ContentResponder ContentResponder
        {
            get
            {
                return _contentResponder;
            }
            set
            {
                _contentResponder = value;
                SetHandlebarsDirectories();
            }
        }

        public string ApplicationName
        {
            get
            {
                return AppContentResponder.ApplicationName;
            }
        }

        public string Render(string templateName, object data)
        {
            throw new NotImplementedException();
        }

        protected override void SetHandlebarsDirectories()
        {
            if(ContentResponder != null)
            {
                base.SetHandlebarsDirectories();
            }
            if(AppContentResponder != null)
            {
                foreach (string templateDirectoryName in AppContentResponder.TemplateDirectoryNames)
                {
                    string directoryPath = Path.Combine(AppContentResponder.AppRoot.Root, templateDirectoryName);
                    HandlebarsDirectories.Add(new HandlebarsDirectory(directoryPath));
                }
            }
        }
    }
}
