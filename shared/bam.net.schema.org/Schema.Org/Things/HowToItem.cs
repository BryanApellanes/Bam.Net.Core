using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>An item used as either a tool or supply when performing the instructions for how to to achieve a result.</summary>
	public class HowToItem: ListItem
	{
		///<summary>The required quantity of the item(s).</summary>
		public OneOfThese<Number,QuantitativeValue,Text> RequiredQuantity {get; set;}
	}
}
