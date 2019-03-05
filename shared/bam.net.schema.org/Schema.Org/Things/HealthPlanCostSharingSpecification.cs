using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A description of costs to the patient under a given network or formulary.</summary>
	public class HealthPlanCostSharingSpecification: Intangible
	{
		///<summary>Whether the coinsurance applies before or after deductible, etc. TODO: Is this a closed set?</summary>
		public Text HealthPlanCoinsuranceOption {get; set;}
		///<summary>Whether The rate of coinsurance expressed as a number between 0.0 and 1.0.</summary>
		public Number HealthPlanCoinsuranceRate {get; set;}
		///<summary>Whether The copay amount.</summary>
		public PriceSpecification HealthPlanCopay {get; set;}
		///<summary>Whether the copay is before or after deductible, etc. TODO: Is this a closed set?</summary>
		public Text HealthPlanCopayOption {get; set;}
		///<summary>The category or type of pharmacy associated with this cost sharing.</summary>
		public Text HealthPlanPharmacyCategory {get; set;}
	}
}
