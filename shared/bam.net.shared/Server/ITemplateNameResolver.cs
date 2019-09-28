using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Server
{
    public interface ITemplateNameResolver
    {
        string ResolveTemplateName(ITemplateable templateable);
        string ResolveTemplateName(object toBeTemplated);
    }
}
