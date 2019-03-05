using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A US-style health insurance plan network.</summary>
	public class HealthPlanNetwork: Intangible
	{
		///<summary>Whether The costs to the patient for services under this network or formulary.</summary>
		public Boolean HealthPlanCostSharing {get; set;}
		///<summary>Name or unique ID of network. (Networks are often reused across different insurance plans).</summary>
		public Text HealthPlanNetworkId {get; set;}
		///<summary>The tier(s) for this network.</summary>
		public Text HealthPlanNetworkTier {get; set;}
	}
}
