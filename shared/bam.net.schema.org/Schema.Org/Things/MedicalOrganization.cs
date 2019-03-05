using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A medical organization (physical or not), such as hospital, institution or clinic.</summary>
	public class MedicalOrganization: Organization
	{
		///<summary>Name or unique ID of network. (Networks are often reused across different insurance plans).</summary>
		public Text HealthPlanNetworkId {get; set;}
		///<summary>Whether the provider is accepting new patients.</summary>
		public Boolean IsAcceptingNewPatients {get; set;}
		///<summary>A medical specialty of the provider.</summary>
		public MedicalSpecialty MedicalSpecialty {get; set;}
	}
}
