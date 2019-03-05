using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A common pathway for the electrochemical nerve impulses that are transmitted along each of the axons.</summary>
	public class Nerve: AnatomicalStructure
	{
		///<summary>The neurological pathway extension that involves muscle control.</summary>
		public Muscle NerveMotor {get; set;}
		///<summary>The neurological pathway extension that inputs and sends information to the brain or spinal cord.</summary>
		public OneOfThese<AnatomicalStructure,SuperficialAnatomy> SensoryUnit {get; set;}
		///<summary>The neurological pathway that originates the neurons.</summary>
		public BrainStructure SourcedFrom {get; set;}
	}
}
