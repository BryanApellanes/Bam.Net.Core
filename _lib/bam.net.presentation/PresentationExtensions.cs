using System.Xml.Linq;

namespace Bam.Net.Presentation
{
    public static class PresentationExtensions
    {
        public static string BamDataAttribute(this XElement element, string bamDataAttributeNameMinusPrefix)
        {
            XAttribute attribute = element.Attribute($"data-bam-{bamDataAttributeNameMinusPrefix}");
            if (attribute == null)
            {
                return string.Empty;
            }

            return attribute.Value;
        }
        
        public static string DataAttribute(this XElement element, string dataAttributeNameMinusPrefix)
        {
            XAttribute attribute = element.Attribute($"data-{dataAttributeNameMinusPrefix}");
            if (attribute == null)
            {
                return BamDataAttribute(element, dataAttributeNameMinusPrefix);
            }
            
            return attribute.Value;
        }
        
    }
}