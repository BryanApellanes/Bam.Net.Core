using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net
{
    public interface IRenderer
    {
        string Render(object toRender);
    }
}
