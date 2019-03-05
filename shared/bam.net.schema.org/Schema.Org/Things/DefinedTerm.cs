using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A word, name, acronym, phrase, etc. with a formal definition. Often used in the context of category or subject classification, glossaries or dictionaries, product or creative work types, etc. Use the name property for the term being defined, use termCode if the term has an alpha-numeric code allocated, use description to provide the definition of the term.</summary>
	public class DefinedTerm: Intangible
	{
		///<summary>A DefinedTermSet that contains this term.</summary>
		public OneOfThese<DefinedTermSet,Url> InDefinedTermSet {get; set;}
		///<summary>A code that identifies this DefinedTerm within a DefinedTermSet</summary>
		public Text TermCode {get; set;}
	}
}
