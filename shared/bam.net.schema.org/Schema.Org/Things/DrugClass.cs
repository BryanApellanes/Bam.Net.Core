using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A class of medical drugs, e.g., statins. Classes can represent general pharmacological class, common mechanisms of action, common physiological effects, etc.</summary>
	public class DrugClass: MedicalEnumeration
	{
		///<summary>Specifying a drug or medicine used in a medication procedure</summary>
		public Drug Drug {get; set;}
	}
}
