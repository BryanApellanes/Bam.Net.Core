using Bam.Net.Presentation.Handlebars;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bam.Net
{
    public static class Handlebars
    {
        public static string Render(string templateName, object renderModel)
        {
            MemoryStream ms = new MemoryStream();
            Render(templateName, renderModel, ms);
            ms.Seek(0, SeekOrigin.Begin);
            return ms.ReadToEnd();
        }

        public static HandlebarsDirectory HandlebarsDirectory { get; set; }
        public static HandlebarsEmbeddedResources HandlebarsEmbeddedResources { get; set; }

        public static void Render(string templateName, object renderModel, Stream output)
        {
            if ((HandlebarsDirectory?.Templates?.ContainsKey(templateName)) == true)
            {
                string code = HandlebarsDirectory.Render(templateName, renderModel);

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
    }
}
