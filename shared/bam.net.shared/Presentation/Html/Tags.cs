namespace Bam.Net.Presentation.Html
{
    public class Tags
    { 
        public static Tag A(object attributes = null, string content = null)
        {
            return new Tag("a", attributes, content);
        }

        public static Tag Div(Tag content)
        {
            return Div(new object(), content.Render());
        }
        
        public static Tag Div(object attributes, string content = null)
        {
            return new Tag("div", attributes, content);
        }

        public static Tag Span(object attributes, string content = null)
        {
            return new Tag("span", attributes, content);
        }
    }
}