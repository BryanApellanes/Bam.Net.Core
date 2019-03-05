using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A financial product for the loaning of an amount of money under agreed terms and charges.</summary>
	public class LoanOrCredit: FinancialProduct
	{
		///<summary>The amount of money.</summary>
		public OneOfThese<MonetaryAmount,Number> Amount {get; set;}
		///<summary>The currency in which the monetary amount is expressed.Use standard formats: ISO 4217 currency format e.g. "USD"; Ticker symbol for cryptocurrencies e.g. "BTC"; well known names for Local Exchange Tradings Systems (LETS) and other currency types e.g. "Ithaca HOUR".</summary>
		public Text Currency {get; set;}
		///<summary>The period of time after any due date that the borrower has to fulfil its obligations before a default (failure to pay) is deemed to have occurred.</summary>
		public Duration GracePeriod {get; set;}
		///<summary>A form of paying back money previously borrowed from a lender. Repayment usually takes the form of periodic payments that normally include part principal plus interest in each payment.</summary>
		public RepaymentSpecification LoanRepaymentForm {get; set;}
		///<summary>The duration of the loan or credit agreement.</summary>
		public QuantitativeValue LoanTerm {get; set;}
		///<summary>The type of a loan or credit.</summary>
		public OneOfThese<Text,Url> LoanType {get; set;}
		///<summary>The only way you get the money back in the event of default is the security. Recourse is where you still have the opportunity to go back to the borrower for the rest of the money.</summary>
		public Boolean RecourseLoan {get; set;}
		///<summary>Whether the terms for payment of interest can be renegotiated during the life of the loan.</summary>
		public Boolean RenegotiableLoan {get; set;}
		///<summary>Assets required to secure loan or credit repayments. It may take form of third party pledge, goods, financial instruments (cash, securities, etc.)</summary>
		public OneOfThese<Text,Thing> RequiredCollateral {get; set;}
	}
}
