using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A medical study is an umbrella type covering all kinds of research studies relating to human medicine or health, including observational studies and interventional trials and registries, randomized, controlled or not. When the specific type of study is known, use one of the extensions of this type, such as MedicalTrial or MedicalObservationalStudy. Also, note that this type should be used to mark up data that describes the study itself; to tag an article that publishes the results of a study, use MedicalScholarlyArticle. Note: use the code property of MedicalEntity to store study IDs, e.g. clinicaltrials.gov ID.</summary>
	public class MedicalStudy: MedicalEntity
	{
		///<summary>Specifying the health condition(s) of a patient, medical study, or other target audience.</summary>
		public MedicalCondition HealthCondition {get; set;}
		///<summary>Expected or actual outcomes of the study.</summary>
		public OneOfThese<MedicalEntity,Text> Outcome {get; set;}
		///<summary>Any characteristics of the population used in the study, e.g. 'males under 65'.</summary>
		public Text Population {get; set;}
		///<summary>A person or organization that supports a thing through a pledge, promise, or financial contribution. e.g. a sponsor of a Medical Study or a corporate sponsor of an event.</summary>
		public OneOfThese<Organization,Person> Sponsor {get; set;}
		///<summary>The status of the study (enumerated).</summary>
		public OneOfThese<EventStatusType,MedicalStudyStatus,Text> Status {get; set;}
		///<summary>The location in which the study is taking/took place.</summary>
		public AdministrativeArea StudyLocation {get; set;}
		///<summary>A subject of the study, i.e. one of the medical conditions, therapies, devices, drugs, etc. investigated by the study.</summary>
		public MedicalEntity StudySubject {get; set;}
	}
}
