using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Presentation.Components
{
    public class InlineComponent: LeafInline, IComponent
    {
        public InlineComponent()
        {
        }

        public InlineComponent(ComponentOptions options)
        {
            Options = options;
        }

        public ComponentOptions Options { get; set; }
        public string Name { get; set; }
        public IComponent ParentComponent { get; set; }
        public List<IComponent> Children { get; set; }
    }
}
