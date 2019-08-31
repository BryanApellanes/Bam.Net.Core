/*
	Copyright © Bryan Apellanes 2015  
*/
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Bam.Net.Presentation.Html
{
    public partial class HiddenAttribute: StringInputAttribute
    {
        public override Tag CreateInput(object data = null)
        {
            return CreateInput("hidden").SetAttribute("name", PropertyName)
                .ValueIf(Default != null, (string)Default);
        }
    }
}
