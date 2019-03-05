using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A structured value representing repayment.</summary>
	public class RepaymentSpecification: StructuredValue
	{
		///<summary>a type of payment made in cash during the onset of the purchase of an expensive good/service. The payment typically represents only a percentage of the full purchase price.</summary>
		public OneOfThese<MonetaryAmount,Number> DownPayment {get; set;}
		///<summary>The amount to be paid as a penalty in the event of early payment of the loan.</summary>
		public MonetaryAmount EarlyPrepaymentPenalty {get; set;}
		///<summary>The amount of money to pay in a single payment.</summary>
		public MonetaryAmount LoanPaymentAmount {get; set;}
		///<summary>Frequency of payments due, i.e. number of months between payments. This is defined as a frequency, i.e. the reciprocal of a period of time.</summary>
		public Number LoanPaymentFrequency {get; set;}
		///<summary>The number of payments contractually required at origination to repay the loan. For monthly paying loans this is the number of months from the contractual first payment date to the maturity date.</summary>
		public Number NumberOfLoanPayments {get; set;}
	}
}
