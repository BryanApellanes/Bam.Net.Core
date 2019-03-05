using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A guideline recommendation that is regarded as efficacious and where quality of the data supporting the recommendation is sound.</summary>
	public class MedicalGuidelineRecommendation: MedicalGuideline
	{
		///<summary>Strength of the guideline's recommendation (e.g. 'class I').</summary>
		public Text RecommendationStrength {get; set;}
	}
}
