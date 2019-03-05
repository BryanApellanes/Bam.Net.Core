using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A web page. Every web page is implicitly assumed to be declared to be of type WebPage, so the various properties about that webpage, such as breadcrumb may be used. We recommend explicit declaration if these properties are specified, but if they are found outside of an itemscope, they will be assumed to be about the page.</summary>
	public class WebPage: CreativeWork
	{
		///<summary>A set of links that can help a user understand and navigate a website hierarchy.</summary>
		public OneOfThese<BreadcrumbList,Text> Breadcrumb {get; set;}
		///<summary>Date on which the content on this web page was last reviewed for accuracy and/or completeness.</summary>
		public Bam.Net.Schema.Org.DataTypes.Date LastReviewed {get; set;}
		///<summary>Indicates if this web page element is the main subject of the page. Supersedes aspect.</summary>
		public WebPageElement MainContentOfPage {get; set;}
		///<summary>Indicates the main image on the page.</summary>
		public ImageObject PrimaryImageOfPage {get; set;}
		///<summary>A link related to this web page, for example to other related web pages.</summary>
		public Url RelatedLink {get; set;}
		///<summary>People or organizations that have reviewed the content on this web page for accuracy and/or completeness.</summary>
		public OneOfThese<Organization,Person> ReviewedBy {get; set;}
		///<summary>One of the more significant URLs on the page. Typically, these are the non-navigation links that are clicked on the most. Supersedes significantLinks.</summary>
		public Url SignificantLink {get; set;}
		///<summary>Indicates sections of a Web page that are particularly 'speakable' in the sense of being highlighted as being especially appropriate for text-to-speech conversion. Other sections of a page may also be usefully spoken in particular circumstances; the 'speakable' property serves to indicate the parts most likely to be generally useful for speech.The speakable property can be repeated an arbitrary number of times, with three kinds of possible 'content-locator' values:1.) id-value URL references - uses id-value of an element in the page being annotated. The simplest use of speakable has (potentially relative) URL values, referencing identified sections of the document concerned.2.) CSS Selectors - addresses content in the annotated page, eg. via class attribute. Use the cssSelector property.3.)  XPaths - addresses content via XPaths (assuming an XML view of the content). Use the xpath property.For more sophisticated markup of speakable sections beyond simple ID references, either CSS selectors or XPath expressions to pick out document section(s) as speakable. For thiswe define a supporting type, SpeakableSpecification  which is defined to be a possible value of the speakable property.</summary>
		public OneOfThese<SpeakableSpecification,Url> Speakable {get; set;}
		///<summary>One of the domain specialities to which this web page's content applies.</summary>
		public Specialty Specialty {get; set;}
	}
}
