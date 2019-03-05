using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A supply consumed when performing the instructions for how to achieve a result.</summary>
	public class HowToSupply: HowToItem
	{
		///<summary>The estimated cost of the supply or supplies consumed when performing instructions.</summary>
		public OneOfThese<MonetaryAmount,Text> EstimatedCost {get; set;}
	}
}
