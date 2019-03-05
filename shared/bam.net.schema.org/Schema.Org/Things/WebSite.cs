using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A WebSite is a set of related web pages and other items typically served from a single web domain and accessible via URLs.</summary>
	public class WebSite: CreativeWork
	{
		///<summary>The International Standard Serial Number (ISSN) that identifies this serial publication. You can repeat this property to identify different formats of, or the linking ISSN (ISSN-L) for, this serial publication.</summary>
		public Text Issn {get; set;}
	}
}
