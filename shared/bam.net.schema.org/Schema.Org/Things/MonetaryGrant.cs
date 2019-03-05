using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A monetary grant.</summary>
	public class MonetaryGrant: Grant
	{
		///<summary>The amount of money.</summary>
		public OneOfThese<MonetaryAmount,Number> Amount {get; set;}
		///<summary>A person or organization that supports (sponsors) something through some kind of financial contribution.</summary>
		public OneOfThese<Organization,Person> Funder {get; set;}
	}
}
