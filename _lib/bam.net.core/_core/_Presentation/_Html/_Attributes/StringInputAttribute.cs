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
    /// <summary>
    /// The base class used for rendering inputs for string object 
    /// properties.
    /// </summary>
    public abstract partial class StringInputAttribute: Attribute
    {        
        public abstract Tag CreateInput(object data = null);

        protected Tag CreateInput(string inputType)
        {
            return Tags.Input(new {type = inputType});
        }
    }
}
