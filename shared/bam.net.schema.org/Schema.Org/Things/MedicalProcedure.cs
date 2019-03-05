using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A process of care used in either a diagnostic, therapeutic, preventive or palliative capacity that relies on invasive (surgical), non-invasive, or other techniques.</summary>
	public class MedicalProcedure: MedicalEntity
	{
		///<summary>Location in the body of the anatomical structure.</summary>
		public Text BodyLocation {get; set;}
		///<summary>Typical or recommended followup care after the procedure is performed.</summary>
		public Text Followup {get; set;}
		///<summary>How the procedure is performed.</summary>
		public Text HowPerformed {get; set;}
		///<summary>A factor that indicates use of this therapy for treatment and/or prevention of a condition, symptom, etc. For therapies such as drugs, indications can include both officially-approved indications as well as off-label uses. These can be distinguished by using the ApprovedIndication subtype of MedicalIndication.</summary>
		public MedicalIndication Indication {get; set;}
		///<summary>Expected or actual outcomes of the study.</summary>
		public OneOfThese<MedicalEntity,Text> Outcome {get; set;}
		///<summary>Typical preparation that a patient must undergo before having the procedure performed.</summary>
		public OneOfThese<MedicalEntity,Text> Preparation {get; set;}
		///<summary>The type of procedure, for example Surgical, Noninvasive, or Percutaneous.</summary>
		public MedicalProcedureType ProcedureType {get; set;}
		///<summary>The status of the study (enumerated).</summary>
		public OneOfThese<EventStatusType,MedicalStudyStatus,Text> Status {get; set;}
	}
}
