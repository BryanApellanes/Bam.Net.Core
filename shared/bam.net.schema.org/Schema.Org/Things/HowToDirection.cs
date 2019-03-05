using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A direction indicating a single action to do in the instructions for how to achieve a result.</summary>
	public class HowToDirection: CreativeWork
	{
		///<summary>A media object representing the circumstances after performing this direction.</summary>
		public OneOfThese<MediaObject,Url> AfterMedia {get; set;}
		///<summary>A media object representing the circumstances before performing this direction.</summary>
		public OneOfThese<MediaObject,Url> BeforeMedia {get; set;}
		///<summary>A media object representing the circumstances while performing this direction.</summary>
		public OneOfThese<MediaObject,Url> DuringMedia {get; set;}
		///<summary>The length of time it takes to perform instructions or a direction (not including time to prepare the supplies), in ISO 8601 duration format.</summary>
		public Duration PerformTime {get; set;}
		///<summary>The length of time it takes to prepare the items to be used in instructions or a direction, in ISO 8601 duration format.</summary>
		public Duration PrepTime {get; set;}
		///<summary>A sub-property of instrument. A supply consumed when performing instructions or a direction.</summary>
		public OneOfThese<HowToSupply,Text> Supply {get; set;}
		///<summary>A sub property of instrument. An object used (but not consumed) when performing instructions or a direction.</summary>
		public OneOfThese<HowToTool,Text> Tool {get; set;}
		///<summary>The total time required to perform instructions or a direction (including time to prepare the supplies), in ISO 8601 duration format.</summary>
		public Duration TotalTime {get; set;}
	}
}
