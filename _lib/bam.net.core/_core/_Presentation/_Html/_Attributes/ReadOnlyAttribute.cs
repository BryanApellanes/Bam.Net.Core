/*
	Copyright © Bryan Apellanes 2015  
*/
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bam.Net.Presentation.Html
{
    public partial class ReadOnlyAttribute: StringInputAttribute
    {
        public override Tag CreateInput(object data = null)
        {
            return Tags.Span(new {name = PropertyName})
                .TextIf(Default != null, (string)Default);
        }
    }
}
