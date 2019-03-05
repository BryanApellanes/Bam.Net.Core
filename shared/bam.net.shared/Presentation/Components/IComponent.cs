using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Presentation.Components
{
    public interface IComponent
    {
        ComponentOptions Options { get; set; }
        string Name { get; set; }

        IComponent ParentComponent { get; set; }
        List<IComponent> Children { get; set; }
    }
}
