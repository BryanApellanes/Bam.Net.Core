using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A product or service offered by a bank whereby one may deposit, withdraw or transfer money and in some cases be paid interest.</summary>
	public class BankAccount: FinancialProduct
	{
		///<summary>A minimum amount that has to be paid in every month.</summary>
		public MonetaryAmount AccountMinimumInflow {get; set;}
		///<summary>An overdraft is an extension of credit from a lending institution when an account reaches zero. An overdraft allows the individual to continue withdrawing money even if the account has no funds in it. Basically the bank allows people to borrow a set amount of money.</summary>
		public MonetaryAmount AccountOverdraftLimit {get; set;}
		///<summary>The type of a bank account.</summary>
		public OneOfThese<Text,Url> BankAccountType {get; set;}
	}
}
