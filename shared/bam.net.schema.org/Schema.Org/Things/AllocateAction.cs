using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>The act of organizing tasks/objects/events by associating resources to it.</summary>
	public class AllocateAction: OrganizeAction
	{
		///<summary>A goal towards an action is taken. Can be concrete or abstract.</summary>
		public OneOfThese<MedicalDevicePurpose,Thing> Purpose {get; set;}
	}
}
