using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A Role that represents a Web link e.g. as expressed via the 'url' property. Its linkRelationship property can indicate URL-based and plain textual link types e.g. those in IANA link registry or others such as 'amphtml'. This structure provides a placeholder where details from HTML's link element can be represented outside of HTML, e.g. in JSON-LD feeds.</summary>
	public class LinkRole: Role
	{
		///<summary>The language of the content or performance or used in an action. Please use one of the language codes from the IETF BCP 47 standard. See also availableLanguage. Supersedes language.</summary>
		public OneOfThese<Language,Text> InLanguage {get; set;}
		///<summary>Indicates the relationship type of a Web link.</summary>
		public Text LinkRelationship {get; set;}
	}
}
