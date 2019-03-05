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
    public partial class ReadOnly: StringInput
    {
        public override TagBuilder CreateInput()
        {
            return new TagBuilder("span")
                .Name(PropertyName)
                .TextIf(Default != null, (string)Default);
        }
    }
}
