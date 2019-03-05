using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A payment method using a credit, debit, store or other card to associate the payment with an account.</summary>
	public class PaymentCard: PaymentMethod
	{
		///<summary>A cardholder benefit that pays the cardholder a small percentage of their net expenditures.</summary>
		public OneOfThese<Boolean,Number> CashBack {get; set;}
		///<summary>A secure method for consumers to purchase products or services via debit, credit or smartcards by using RFID or NFC technology.</summary>
		public Boolean ContactlessPayment {get; set;}
		///<summary>A floor limit is the amount of money above which credit card transactions must be authorized.</summary>
		public MonetaryAmount FloorLimit {get; set;}
	}
}
