using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A statistical distribution of monetary amounts.</summary>
	public class MonetaryAmountDistribution: QuantitativeValue
	{
		///<summary>The currency in which the monetary amount is expressed.Use standard formats: ISO 4217 currency format e.g. "USD"; Ticker symbol for cryptocurrencies e.g. "BTC"; well known names for Local Exchange Tradings Systems (LETS) and other currency types e.g. "Ithaca HOUR".</summary>
		public Text Currency {get; set;}
	}
}
