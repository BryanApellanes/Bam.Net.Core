using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A tourist destination. In principle any Place can be a TouristDestination from a City, Region or Country to an AmusementPark or Hotel. This Type can be used on its own to describe a general TourstDestination, or be used as an additionalType to add tourist relevant properties to any other Place.  A TouristDestination is defined as a Place that contains, or is colocated with, one or more TourstAttractions, often linked by a similar theme or interest to a particular touristType. The UNWTO defines Destination (main destination of a tourism trip) as the place visited that is central to the decision to take the trip.  (See examples below).</summary>
	public class TouristDestination: Place
	{
		///<summary>Attraction located at destination.</summary>
		public TouristAttraction IncludesAttraction {get; set;}
		///<summary>Attraction suitable for type(s) of tourist. eg. Children, visitors from a particular country, etc.</summary>
		public OneOfThese<Audience,Text> TouristType {get; set;}
	}
}
