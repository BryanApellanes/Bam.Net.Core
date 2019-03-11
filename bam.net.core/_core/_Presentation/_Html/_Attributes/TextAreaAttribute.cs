using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bam.Net.Presentation.Html
{
    public partial class TextAreaAttribute : StringInputAttribute
    {
        public override Tag CreateInput(object data = null)
        {
            return Tags.Textarea()
                            .Attr("rows", this.Rows.ToString())
                            .Attr("cols", this.Cols.ToString())
                            .TextIf(Default != null, (string)Default);
        }
    }
}
