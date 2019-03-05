using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A fact-checking review of claims made (or reported) in some creative work (referenced via itemReviewed).</summary>
	public class ClaimReview: Review
	{
		///<summary>A short summary of the specific claims reviewed in a ClaimReview.</summary>
		public Text ClaimReviewed {get; set;}
	}
}
