using Bam.Net.Presentation.Handlebars;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bam.Net.Logging;

namespace Bam.Net
{
    public class HandlebarsTemplateRenderer : ITemplateRenderer
    {
        public HandlebarsTemplateRenderer(HandlebarsEmbeddedResources handlebarsEmbeddedResources, HandlebarsDirectory handlebarsDirectory)
            : this(handlebarsEmbeddedResources, new HandlebarsDirectory[] {handlebarsDirectory})
        {
        }
        
        public HandlebarsTemplateRenderer(HandlebarsEmbeddedResources handlebarsEmbeddedResources, params HandlebarsDirectory[] handlebarsDirectories)
        {
            HandlebarsDirectories = new HashSet<HandlebarsDirectory>();
            HandlebarsEmbeddedResources = handlebarsEmbeddedResources;
            
            HandlebarsEmbeddedResources.Reload();
            
            foreach (HandlebarsDirectory handlebarsDirectory in handlebarsDirectories)
            {
                handlebarsDirectory.Reload();
                HandlebarsDirectories.Add(handlebarsDirectory);
            }
        }

        public ILogger Logger { get; set; }
        
        public HashSet<HandlebarsDirectory> HandlebarsDirectories { get; set; }
        public HandlebarsEmbeddedResources HandlebarsEmbeddedResources { get; set; }

        public void AddDirectory(DirectoryInfo directoryInfo)
        {
            HandlebarsDirectories.Add(new HandlebarsDirectory(directoryInfo, Logger));
        }

        public string Render(string templateName, object data)
        {
            MemoryStream ms = new MemoryStream();
            Render(templateName, data, ms);
            ms.Seek(0, SeekOrigin.Begin);
            return ms.ReadToEnd();
        }
        
        public void Render(object toRender, Stream output)
        {
            Args.ThrowIfNull(toRender, "toRender");
            Render(toRender.GetType().Name, toRender, output);
        }

        public void Render(string templateName, object renderModel, Stream output)
        {
            HandlebarsDirectory handlebarsDirectory = GetHandlebarsDirectory(templateName);
            if (handlebarsDirectory != null)
            { 
                string code = handlebarsDirectory.Render(templateName, renderModel);
                code.WriteToStream(output);
            }
            else if ((HandlebarsEmbeddedResources?.Templates?.ContainsKey(templateName)) == true)
            {
                string code = HandlebarsEmbeddedResources.Render(templateName, renderModel);
                code.WriteToStream(output);
            }
            else
            {
                Args.Throw<InvalidOperationException>("Specified template {0} not found", templateName);
            }
        }

        private HandlebarsDirectory GetHandlebarsDirectory(string templateName)
        {
            HandlebarsDirectory toUse = HandlebarsDirectories.FirstOrDefault(h => h.HasTemplate(templateName));
            if (HandlebarsDirectories.Count(h => h.HasTemplate(templateName)) > 1)
            {
                (Logger ?? Log.Default).Info("Multiple templates named {0} were found, using {1}", templateName, Path.Combine(toUse.Directory.FullName, templateName));
            }

            return toUse;
        }
    }
}
