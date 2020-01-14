using System.Reflection;
using static Bam.Net.Presentation.Html.Tags;

namespace Bam.Net.Presentation.Html
{
    public class StringInputProvider: InputProvider
    {
        public override Tag CreateInput(PropertyInfo propertyInfo, object data = null)
        {
            if (propertyInfo.HasCustomAttributeOfType<StringInputAttribute>(out StringInputAttribute inputAttribute))
            {
                return inputAttribute.CreateInput(data);
            }

            return Input(new {type = "text", name = propertyInfo.Name});
        }
    }
}