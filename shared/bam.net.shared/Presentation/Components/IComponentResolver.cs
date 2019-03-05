using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Presentation.Components
{
    public interface IComponentResolver
    {
        bool IsValidComponentName(string componentName, out LeafInline component);
    }
}
