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
    public partial class Password: StringInput
    {
        public override TagBuilder CreateInput()
        {
            return CreateInput("password")
                .Name(PropertyName);
        }
    }
}
