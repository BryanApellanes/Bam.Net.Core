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
    public partial class PasswordAttribute: StringInputAttribute
    {
        public override Tag CreateInput(object data = null)
        {
            return CreateInput("password").SetAttribute("name", PropertyName);
        }
    }
}
