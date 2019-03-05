using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A US-style health insurance plan, including PPOs, EPOs, and HMOs.</summary>
	public class HealthInsurancePlan: Intangible
	{
		///<summary>The URL that goes directly to the summary of benefits and coverage for the specific standard plan or plan variation.</summary>
		public Url BenefitsSummaryUrl {get; set;}
		///<summary>A contact point for a person or organization. Supersedes contactPoints.</summary>
		public ContactPoint ContactPoint {get; set;}
		///<summary>TODO.</summary>
		public Text HealthPlanDrugOption {get; set;}
		///<summary>The tier(s) of drugs offered by this formulary or insurance plan.</summary>
		public Text HealthPlanDrugTier {get; set;}
		///<summary>The 14-character, HIOS-generated Plan ID number. (Plan IDs must be unique, even across different markets.)</summary>
		public Text HealthPlanId {get; set;}
		///<summary>The URL that goes directly to the plan brochure for the specific standard plan or plan variation.</summary>
		public Url HealthPlanMarketingUrl {get; set;}
		///<summary>Formularies covered by this plan.</summary>
		public HealthPlanFormulary IncludesHealthPlanFormulary {get; set;}
		///<summary>Networks covered by this plan.</summary>
		public HealthPlanNetwork IncludesHealthPlanNetwork {get; set;}
		///<summary>The standard for interpreting thePlan ID. The preferred is "HIOS". See the Centers for Medicare & Medicaid Services for more details.</summary>
		public OneOfThese<Text,Url> UsesHealthPlanIdStandard {get; set;}
	}
}
