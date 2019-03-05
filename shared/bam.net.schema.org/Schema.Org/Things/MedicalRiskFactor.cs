using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A risk factor is anything that increases a person's likelihood of developing or contracting a disease, medical condition, or complication.</summary>
	public class MedicalRiskFactor: MedicalEntity
	{
		///<summary>The condition, complication, etc. influenced by this factor.</summary>
		public MedicalEntity IncreasesRiskOf {get; set;}
	}
}
