using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A SpeakableSpecification indicates (typically via xpath or cssSelector) sections of a document that are highlighted as particularly speakable. Instances of this type are expected to be used primarily as values of the speakable property.</summary>
	public class SpeakableSpecification: Intangible
	{
		///<summary>A CSS selector, e.g. of a SpeakableSpecification or WebPageElement. In the latter case, multiple matches within a page can constitute a single conceptual "Web page element".</summary>
		public string CssSelector {get; set;}
		///<summary>An XPath, e.g. of a SpeakableSpecification or WebPageElement. In the latter case, multiple matches within a page can constitute a single conceptual "Web page element".</summary>
		public string Xpath {get; set;}
	}
}
