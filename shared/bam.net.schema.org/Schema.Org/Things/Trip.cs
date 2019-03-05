using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A trip or journey. An itinerary of visits to one or more places.</summary>
	public class Trip: Intangible
	{
		///<summary>The expected arrival time.</summary>
		public Bam.Net.Schema.Org.DataTypes.Date ArrivalTime {get; set;}
		///<summary>The expected departure time.</summary>
		public Bam.Net.Schema.Org.DataTypes.Date DepartureTime {get; set;}
		///<summary>Indicates an item or CreativeWork that is part of this item, or CreativeWork (in some sense). Inverse property: isPartOf.</summary>
		public OneOfThese<CreativeWork,Trip> HasPart {get; set;}
		///<summary>Indicates an item or CreativeWork that this item, or CreativeWork (in some sense), is part of. Inverse property: hasPart.</summary>
		public OneOfThese<CreativeWork,Trip> IsPartOf {get; set;}
		///<summary>Destination(s) ( Place ) that make up a trip. For a trip where destination order is important use ItemList to specify that order (see examples).</summary>
		public OneOfThese<ItemList,Place> Itinerary {get; set;}
		///<summary>An offer to provide this item—for example, an offer to sell a product, rent the DVD of a movie, perform a service, or give away tickets to an event.</summary>
		public Offer Offers {get; set;}
		///<summary>The service provider, service operator, or service performer; the goods producer. Another party (a seller) may offer those services or goods on behalf of the provider. A provider may also serve as the seller. Supersedes carrier.</summary>
		public OneOfThese<Organization,Person> Provider {get; set;}
	}
}
