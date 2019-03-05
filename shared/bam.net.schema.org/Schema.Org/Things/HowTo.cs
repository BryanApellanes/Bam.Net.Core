using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>Instructions that explain how to achieve a result by performing a sequence of steps.</summary>
	public class HowTo: CreativeWork
	{
		///<summary>The estimated cost of the supply or supplies consumed when performing instructions.</summary>
		public OneOfThese<MonetaryAmount,Text> EstimatedCost {get; set;}
		///<summary>The length of time it takes to perform instructions or a direction (not including time to prepare the supplies), in ISO 8601 duration format.</summary>
		public Duration PerformTime {get; set;}
		///<summary>The length of time it takes to prepare the items to be used in instructions or a direction, in ISO 8601 duration format.</summary>
		public Duration PrepTime {get; set;}
		///<summary>A single step item (as HowToStep, text, document, video, etc.) or a HowToSection. Supersedes steps.</summary>
		public OneOfThese<CreativeWork,HowToSection,HowToStep,Text> Step {get; set;}
		///<summary>A sub-property of instrument. A supply consumed when performing instructions or a direction.</summary>
		public OneOfThese<HowToSupply,Text> Supply {get; set;}
		///<summary>A sub property of instrument. An object used (but not consumed) when performing instructions or a direction.</summary>
		public OneOfThese<HowToTool,Text> Tool {get; set;}
		///<summary>The total time required to perform instructions or a direction (including time to prepare the supplies), in ISO 8601 duration format.</summary>
		public Duration TotalTime {get; set;}
		///<summary>The quantity that results by performing instructions. For example, a paper airplane, 10 personalized candles.</summary>
		public OneOfThese<QuantitativeValue,Text> Yield {get; set;}
	}
}
