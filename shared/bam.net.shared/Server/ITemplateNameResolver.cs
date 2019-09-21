using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Server
{
    public interface ITemplateNameResolver
    {
        string ResolveTemplateName(object toBeTemplated);
    }
}
