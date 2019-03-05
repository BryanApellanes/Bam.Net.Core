using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A patient is any person recipient of health care services.</summary>
	public class PatientMedicalAudience: MedicalAudience
	{
		///<summary>One or more alternative conditions considered in the differential diagnosis process as output of a diagnosis process.</summary>
		public MedicalCondition Diagnosis {get; set;}
		///<summary>Specifying a drug or medicine used in a medication procedure</summary>
		public Drug Drug {get; set;}
		///<summary>Specifying the health condition(s) of a patient, medical study, or other target audience.</summary>
		public MedicalCondition HealthCondition {get; set;}
	}
}
