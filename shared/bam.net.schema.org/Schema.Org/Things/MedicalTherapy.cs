using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>Any medical intervention designed to prevent, treat, and cure human diseases and medical conditions, including both curative and palliative therapies. Medical therapies are typically processes of care relying upon pharmacotherapy, behavioral therapy, supportive therapy (with fluid or nutrition for example), or detoxification (e.g. hemodialysis) aimed at improving or preventing a health condition.</summary>
	public class MedicalTherapy: TherapeuticProcedure
	{
		///<summary>A contraindication for this therapy.</summary>
		public OneOfThese<MedicalContraindication,Text> Contraindication {get; set;}
		///<summary>A therapy that duplicates or overlaps this one.</summary>
		public MedicalTherapy DuplicateTherapy {get; set;}
		///<summary>A possible serious complication and/or serious side effect of this therapy. Serious adverse outcomes include those that are life-threatening; result in death, disability, or permanent damage; require hospitalization or prolong existing hospitalization; cause congenital anomalies or birth defects; or jeopardize the patient and may require medical or surgical intervention to prevent one of the outcomes in this definition.</summary>
		public MedicalEntity SeriousAdverseOutcome {get; set;}
	}
}
