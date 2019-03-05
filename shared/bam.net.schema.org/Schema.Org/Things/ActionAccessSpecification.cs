using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A set of requirements that a must be fulfilled in order to perform an Action.</summary>
	public class ActionAccessSpecification: Intangible
	{
		///<summary>The end of the availability of the product or service included in the offer.</summary>
		public Bam.Net.Schema.Org.DataTypes.Date AvailabilityEnds {get; set;}
		///<summary>The beginning of the availability of the product or service included in the offer.</summary>
		public Bam.Net.Schema.Org.DataTypes.Date AvailabilityStarts {get; set;}
		///<summary>A category for the item. Greater signs or slashes can be used to informally indicate a category hierarchy.</summary>
		public OneOfThese<PhysicalActivityCategory,Text,Thing> Category {get; set;}
		///<summary>The ISO 3166-1 (ISO 3166-1 alpha-2) or ISO 3166-2 code, the place, or the GeoShape for the geo-political region(s) for which the offer or delivery charge specification is valid.See also ineligibleRegion.</summary>
		public OneOfThese<GeoShape,Place,Text> EligibleRegion {get; set;}
		///<summary>An Offer which must be accepted before the user can perform the Action. For example, the user may need to buy a movie before being able to watch it.</summary>
		public Offer ExpectsAcceptanceOf {get; set;}
		///<summary>Indicates if use of the media require a subscription  (either paid or free). Allowed values are true or false (note that an earlier version had 'yes', 'no').</summary>
		public OneOfThese<Boolean,MediaSubscription> RequiresSubscription {get; set;}
	}
}
