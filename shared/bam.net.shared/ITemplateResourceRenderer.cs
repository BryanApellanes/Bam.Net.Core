using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bam.Net
{
    public interface ITemplateResourceRenderer: ITemplateRenderer
    {
        void RenderResource(string templateName, object toRender, Stream output);
    }
}
