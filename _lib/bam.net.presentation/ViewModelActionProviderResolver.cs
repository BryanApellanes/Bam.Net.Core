using System;
using Bam.Net.Server;

namespace Bam.Net.Presentation
{
    public class ViewModelActionProviderResolver: IActionProviderResolver
    {
        // TODO: consider encapsulating ParseActionProviderName here
        public Type ResolveActionProvider(string actionProvider)
        {
            // TODO: See ViewModelFile for implementation that should be moved here
            throw new NotImplementedException();
        }
    }
}