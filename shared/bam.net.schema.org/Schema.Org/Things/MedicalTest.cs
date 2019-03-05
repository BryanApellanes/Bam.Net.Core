using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>Any medical test, typically performed for diagnostic purposes.</summary>
	public class MedicalTest: MedicalEntity
	{
		///<summary>Drugs that affect the test's results.</summary>
		public Drug AffectedBy {get; set;}
		///<summary>Range of acceptable values for a typical patient, when applicable.</summary>
		public OneOfThese<MedicalEnumeration,Text> NormalRange {get; set;}
		///<summary>A sign detected by the test.</summary>
		public MedicalSign SignDetected {get; set;}
		///<summary>A condition the test is used to diagnose.</summary>
		public MedicalCondition UsedToDiagnose {get; set;}
		///<summary>Device used to perform the test.</summary>
		public MedicalDevice UsesDevice {get; set;}
	}
}
