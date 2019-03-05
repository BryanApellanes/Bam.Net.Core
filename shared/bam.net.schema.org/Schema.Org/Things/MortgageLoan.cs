using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A loan in which property or real estate is used as collateral. (A loan securitized against some real estate.)</summary>
	public class MortgageLoan: LoanOrCredit
	{
		///<summary>Whether borrower is a resident of the jurisdiction where the property is located.</summary>
		public Boolean DomiciledMortgage {get; set;}
		///<summary>Amount of mortgage mandate that can be converted into a proper mortgage at a later stage.</summary>
		public MonetaryAmount LoanMortgageMandateAmount {get; set;}
	}
}
