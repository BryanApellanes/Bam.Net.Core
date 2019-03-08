using System;
using System.Collections.Generic;

namespace Bam.Net.Presentation.Html
{
    public class Tags
    { 
        public static Tag A(object attributes = null, string content = null)
        {
            return new Tag("a", attributes, content);
        }

        public static Tag Div(Func<Tag> content)
        {
            return Div(new object(), content);
        }

        public static Tag Div(object attributes, Func<Tag> content)
        {
            return Div(attributes, content()?.Render());
        }
       
        public static Tag Div(Tag content)
        {
            return Div(new object(), content.Render());
        }
        
        public static Tag Div(object attributes = null, string content = null)
        {
            return new Tag("div", attributes, content);
        }

        public static Tag Span(object attributes = null, string content = null)
        {
            return new Tag("span", attributes, content);
        }
    }
}