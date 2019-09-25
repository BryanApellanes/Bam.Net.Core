using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Server
{
    public interface IIncludesResolver
    {
        Includes ResolveCommonIncludes(string contentRoot);
        Includes ResolveApplicationIncludes(string applicationName, string contentRoot);
    }
}
