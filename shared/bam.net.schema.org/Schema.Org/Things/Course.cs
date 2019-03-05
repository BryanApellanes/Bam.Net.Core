using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A description of an educational course which may be offered as distinct instances at which take place at different times or take place at different locations, or be offered through different media or modes of study. An educational course is a sequence of one or more educational events and/or creative works which aims to build knowledge, competence or ability of learners.</summary>
	public class Course: CreativeWork
	{
		///<summary>The identifier for the Course used by the course provider (e.g. CS101 or 6.001).</summary>
		public Text CourseCode {get; set;}
		///<summary>Requirements for taking the Course. May be completion of another Course or a textual description like "permission of instructor". Requirements may be a pre-requisite competency, referenced using AlignmentObject.</summary>
		public OneOfThese<AlignmentObject,Course,Text> CoursePrerequisites {get; set;}
		///<summary>A description of the qualification, award, certificate, diploma or other educational credential awarded as a consequence of successful completion of this course.</summary>
		public OneOfThese<Text,Url> EducationalCredentialAwarded {get; set;}
		///<summary>An offering of the course at a specific time and place or through specific media or mode of study or to a specific section of students.</summary>
		public CourseInstance HasCourseInstance {get; set;}
	}
}
