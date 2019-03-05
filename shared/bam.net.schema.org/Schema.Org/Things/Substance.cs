using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>Any matter of defined composition that has discrete existence, whose origin may be biological, mineral or chemical.</summary>
	public class Substance: MedicalEntity
	{
		///<summary>An active ingredient, typically chemical compounds and/or biologic substances.</summary>
		public Text ActiveIngredient {get; set;}
		///<summary>Recommended intake of this supplement for a given population as defined by a specific recommending authority.</summary>
		public MaximumDoseSchedule MaximumIntake {get; set;}
	}
}
