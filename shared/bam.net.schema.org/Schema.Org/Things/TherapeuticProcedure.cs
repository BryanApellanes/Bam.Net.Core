using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A medical procedure intended primarily for therapeutic purposes, aimed at improving a health condition.</summary>
	public class TherapeuticProcedure: MedicalProcedure
	{
		///<summary>A possible complication and/or side effect of this therapy. If it is known that an adverse outcome is serious (resulting in death, disability, or permanent damage; requiring hospitalization; or is otherwise life-threatening or requires immediate medical attention), tag it as a seriouseAdverseOutcome instead.</summary>
		public MedicalEntity AdverseOutcome {get; set;}
		///<summary>A dosing schedule for the drug for a given population, either observed, recommended, or maximum dose based on the type used.</summary>
		public DoseSchedule DoseSchedule {get; set;}
		///<summary>Specifying a drug or medicine used in a medication procedure</summary>
		public Drug Drug {get; set;}
		///<summary>A factor that indicates use of this therapy for treatment and/or prevention of a condition, symptom, etc. For therapies such as drugs, indications can include both officially-approved indications as well as off-label uses. These can be distinguished by using the ApprovedIndication subtype of MedicalIndication.</summary>
		public MedicalIndication Indication {get; set;}
	}
}
