using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>Any feature associated or not with a medical condition. In medicine a symptom is generally subjective while a sign is objective.</summary>
	public class MedicalSignOrSymptom: MedicalCondition
	{
		///<summary>Specifying a cause of something in general. e.g in medicine , one of the causative agent(s) that are most directly responsible for the pathophysiologic process that eventually results in the occurrence.</summary>
		public MedicalCause Cause {get; set;}
		///<summary>A possible treatment to address this condition, sign or symptom.</summary>
		public MedicalTherapy PossibleTreatment {get; set;}
	}
}
