using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>The frequency in MHz and the modulation used for a particular BroadcastService.</summary>
	public class BroadcastFrequencySpecification: Intangible
	{
		///<summary>The frequency in MHz for a particular broadcast.</summary>
		public OneOfThese<Number,QuantitativeValue> BroadcastFrequencyValue {get; set;}
	}
}
