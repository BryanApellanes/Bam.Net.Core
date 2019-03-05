using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>Used to describe a ticket to an event, a flight, a bus ride, etc.</summary>
	public class Ticket: Intangible
	{
		///<summary>The date the ticket was issued.</summary>
		public Bam.Net.Schema.Org.DataTypes.Date DateIssued {get; set;}
		///<summary>The organization issuing the ticket or permit.</summary>
		public Organization IssuedBy {get; set;}
		///<summary>The currency of the price, or a price component when attached to PriceSpecification and its subtypes.Use standard formats: ISO 4217 currency format e.g. "USD"; Ticker symbol for cryptocurrencies e.g. "BTC"; well known names for Local Exchange Tradings Systems (LETS) and other currency types e.g. "Ithaca HOUR".</summary>
		public Text PriceCurrency {get; set;}
		///<summary>The unique identifier for the ticket.</summary>
		public Text TicketNumber {get; set;}
		///<summary>Reference to an asset (e.g., Barcode, QR code image or PDF) usable for entrance.</summary>
		public OneOfThese<Text,Url> TicketToken {get; set;}
		///<summary>The seat associated with the ticket.</summary>
		public Seat TicketedSeat {get; set;}
		///<summary>The total price for the reservation or ticket, including applicable taxes, shipping, etc.</summary>
		public OneOfThese<Number,PriceSpecification,Text> TotalPrice {get; set;}
		///<summary>The person or organization the reservation or ticket is for.</summary>
		public OneOfThese<Organization,Person> UnderName {get; set;}
	}
}
