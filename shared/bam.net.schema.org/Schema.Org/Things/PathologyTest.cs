using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A medical test performed by a laboratory that typically involves examination of a tissue sample by a pathologist.</summary>
	public class PathologyTest: MedicalTest
	{
		///<summary>The type of tissue sample required for the test.</summary>
		public Text TissueSample {get; set;}
	}
}
