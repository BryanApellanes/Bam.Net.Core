using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Presentation.Handlebars
{
    public class HandlebarsTemplateSet
    {
        public const string DefaultPath = "./Handlebars";
        private readonly HandlebarsTemplateRenderer _renderer;
        public HandlebarsTemplateSet()
        {
            HandlebarsDirectory = new HandlebarsDirectory(DefaultPath);
            HandlebarsEmbeddedResources = new HandlebarsEmbeddedResources(Assembly.GetExecutingAssembly());
            _renderer = new HandlebarsTemplateRenderer(HandlebarsDirectory, HandlebarsEmbeddedResources);
        }

        public HandlebarsTemplateSet(string directoryPath)
        {
            HandlebarsDirectory = new HandlebarsDirectory(directoryPath);
            HandlebarsEmbeddedResources = new HandlebarsEmbeddedResources(Assembly.GetExecutingAssembly());
            _renderer = new HandlebarsTemplateRenderer(HandlebarsDirectory, HandlebarsEmbeddedResources);
        }

        public HandlebarsTemplateSet(Assembly embeddedResourceContainer)
        {
            HandlebarsDirectory = new HandlebarsDirectory(DefaultPath);
            HandlebarsEmbeddedResources = new HandlebarsEmbeddedResources(embeddedResourceContainer);
            _renderer = new HandlebarsTemplateRenderer(HandlebarsDirectory, HandlebarsEmbeddedResources);
        }
        
        public HandlebarsDirectory HandlebarsDirectory { get; set; }
        public HandlebarsEmbeddedResources HandlebarsEmbeddedResources { get; set; }

        public string Render(string templateName, object data)
        {
            MemoryStream ms = new MemoryStream();
            _renderer.Render(templateName, data, ms);
            ms.Seek(0, SeekOrigin.Begin);
            return ms.ReadToEnd();
        }
    }
}