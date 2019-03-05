using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>Any collection of tests commonly ordered together.</summary>
	public class MedicalTestPanel: MedicalTest
	{
		///<summary>A component test of the panel.</summary>
		public MedicalTest SubTest {get; set;}
	}
}
