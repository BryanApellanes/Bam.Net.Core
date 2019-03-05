using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>The act of transferring money from one place to another place. This may occur electronically or physically.</summary>
	public class MoneyTransfer: TransferAction
	{
		///<summary>The amount of money.</summary>
		public OneOfThese<MonetaryAmount,Number> Amount {get; set;}
		///<summary>A bank or bank’s branch, financial institution or international financial institution operating the beneficiary’s bank account or releasing funds for the beneficiary</summary>
		public OneOfThese<BankOrCreditUnion,Text> BeneficiaryBank {get; set;}
	}
}
