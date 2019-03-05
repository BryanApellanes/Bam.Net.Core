using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>The act of ingesting information/resources/food.</summary>
	public class ConsumeAction: Action
	{
		///<summary>A set of requirements that a must be fulfilled in order to perform an Action. If more than one value is specied, fulfilling one set of requirements will allow the Action to be performed.</summary>
		public ActionAccessSpecification ActionAccessibilityRequirement {get; set;}
		///<summary>An Offer which must be accepted before the user can perform the Action. For example, the user may need to buy a movie before being able to watch it.</summary>
		public Offer ExpectsAcceptanceOf {get; set;}
	}
}
