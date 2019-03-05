using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A card payment method of a particular brand or name.  Used to mark up a particular payment method and/or the financial product/service that supplies the card account.Commonly used values:http://purl.org/goodrelations/v1#AmericanExpresshttp://purl.org/goodrelations/v1#DinersClubhttp://purl.org/goodrelations/v1#Discoverhttp://purl.org/goodrelations/v1#JCBhttp://purl.org/goodrelations/v1#MasterCardhttp://purl.org/goodrelations/v1#VISA</summary>
	public class CreditCard: PaymentCard
	{
		///<summary>The minimum payment is the lowest amount of money that one is required to pay on a credit card statement each month.</summary>
		public OneOfThese<MonetaryAmount,Number> MonthlyMinimumRepaymentAmount {get; set;}
	}
}
