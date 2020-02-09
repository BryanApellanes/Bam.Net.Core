using System;

namespace Bam.Net.Server
{
    public interface IActionProviderResolver
    {
        Type ResolveActionProvider(string actionProvider);
    }
}