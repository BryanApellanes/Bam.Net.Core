using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A structured value representing exchange rate.</summary>
	public class ExchangeRateSpecification: StructuredValue
	{
		///<summary>The currency in which the monetary amount is expressed.Use standard formats: ISO 4217 currency format e.g. "USD"; Ticker symbol for cryptocurrencies e.g. "BTC"; well known names for Local Exchange Tradings Systems (LETS) and other currency types e.g. "Ithaca HOUR".</summary>
		public Text Currency {get; set;}
		///<summary>The current price of a currency.</summary>
		public UnitPriceSpecification CurrentExchangeRate {get; set;}
		///<summary>The difference between the price at which a broker or other intermediary buys and sells foreign currency.</summary>
		public OneOfThese<MonetaryAmount,Number> ExchangeRateSpread {get; set;}
	}
}
