using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>For a given health insurance plan, the specification for costs and coverage of prescription drugs.</summary>
	public class HealthPlanFormulary: Intangible
	{
		///<summary>Whether The costs to the patient for services under this network or formulary.</summary>
		public Boolean HealthPlanCostSharing {get; set;}
		///<summary>The tier(s) of drugs offered by this formulary or insurance plan.</summary>
		public Text HealthPlanDrugTier {get; set;}
		///<summary>Whether prescriptions can be delivered by mail.</summary>
		public Boolean OffersPrescriptionByMail {get; set;}
	}
}
