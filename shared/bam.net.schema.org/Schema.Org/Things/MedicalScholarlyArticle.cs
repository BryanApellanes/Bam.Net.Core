using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A scholarly article in the medical domain.</summary>
	public class MedicalScholarlyArticle: ScholarlyArticle
	{
		///<summary>The type of the medical article, taken from the US NLM MeSH publication type catalog. See also MeSH documentation.</summary>
		public Text PublicationType {get; set;}
	}
}
