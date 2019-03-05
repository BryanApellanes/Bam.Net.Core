using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A tourist trip. A created itinerary of visits to one or more places of interest (TouristAttraction/TouristDestination) often linked by a similar theme, geographic area, or interest to a particular touristType. The UNWTO defines tourism trip as the Trip taken by visitors.  (See examples below).</summary>
	public class TouristTrip: Trip
	{
		///<summary>Attraction suitable for type(s) of tourist. eg. Children, visitors from a particular country, etc.</summary>
		public OneOfThese<Audience,Text> TouristType {get; set;}
	}
}
