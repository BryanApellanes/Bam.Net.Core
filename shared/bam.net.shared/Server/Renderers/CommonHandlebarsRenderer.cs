using Bam.Net.Presentation;
using Bam.Net.Presentation.Handlebars;
using Bam.Net.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Bam.Net.Server.Renderers
{
    public class CommonHandlebarsRenderer : WebRenderer, ITemplateManager
    {
        public CommonHandlebarsRenderer(ContentResponder contentResponder)
            : base("text/html", ".htm", ".html")
        {
            ContentResponder = contentResponder;
            HandlebarsDirectories = new HashSet<HandlebarsDirectory>();
            SetHandlebarsDirectories();
            HandlebarsEmbeddedResources = new HandlebarsEmbeddedResources(Assembly.GetEntryAssembly());
        }

        public string ContentRoot
        {
            get
            {
                return ContentResponder.ContentRoot;
            }
        }

        public ITemplateNameResolver TemplateNameResolver
        {
            get
            {
                return ContentResponder?.ApplicationServiceRegistry?.Get<ITemplateNameResolver>() ?? ApplicationServiceRegistry.Current?.Get<ITemplateNameResolver>();
            }
        }

        public HashSet<HandlebarsDirectory> HandlebarsDirectories
        {
            get;
            private set;
        }

        public HandlebarsEmbeddedResources HandlebarsEmbeddedResources
        {
            get;
        }

        HandlebarsDirectory _combined;
        object _combinedLock = new object();
        protected HandlebarsDirectory CombinedHandlebarsDirectory()
        {
            return _combinedLock.DoubleCheckLock(ref _combined, () =>
            {
                _combined = HandlebarsDirectories.First();
                return _combined.CombineWith(HandlebarsDirectories.ToArray());
            });
        }

        public override void Render(object toRender, Stream output)
        {
            string templateName = TemplateNameResolver.ResolveTemplateName(toRender);
            CombinedHandlebarsDirectory().Render(templateName, toRender).WriteToStream(output);
        }

        public virtual void RenderLayout(LayoutModel toRender, Stream output)
        {
            string templateName = TemplateNameResolver.ResolveTemplateName(toRender);

            HandlebarsDirectory combined = CombinedHandlebarsDirectory();
            string rendered = combined.Render(toRender.LayoutName, toRender);
            StreamWriter streamWriter = new StreamWriter(output);
            streamWriter.Write(rendered);
        }

        protected virtual void SetHandlebarsDirectories()
        {
            HashSet<HandlebarsDirectory> handlebarsDirectories = new HashSet<HandlebarsDirectory>
            {
                new HandlebarsDirectory(Path.Combine(ContentRoot, "common", "views", "layouts"))
            };
            foreach (string templateDirectory in ContentResponder.TemplateDirectoryNames)
            {
                string directoryPath = Path.Combine(ContentRoot, "common", templateDirectory);
                handlebarsDirectories.Add(new HandlebarsDirectory(directoryPath));
            }
            HandlebarsDirectories = handlebarsDirectories;
        }
        
        public string Render(string templateName, object data)
        {
            throw new NotImplementedException();
        }
    }
}
