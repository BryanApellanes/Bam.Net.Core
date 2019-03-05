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
    public abstract partial class StringInput: Attribute
    {        
        public abstract TagBuilder CreateInput();

        protected TagBuilder CreateInput(string inputType)
        {
            return new TagBuilder("input")
                .Type(inputType);
        }
    }
}
