using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>An instance of a Course which is distinct from other instances because it is offered at a different time or location or through different media or modes of study or to a specific section of students.</summary>
	public class CourseInstance: Event
	{
		///<summary>The medium or means of delivery of the course instance or the mode of study, either as a text label (e.g. "online", "onsite" or "blended"; "synchronous" or "asynchronous"; "full-time" or "part-time") or as a URL reference to a term from a controlled vocabulary (e.g. https://ceds.ed.gov/element/001311#Asynchronous ).</summary>
		public OneOfThese<Text,Url> CourseMode {get; set;}
		///<summary>A person assigned to instruct or provide instructional assistance for the CourseInstance.</summary>
		public Person Instructor {get; set;}
	}
}
