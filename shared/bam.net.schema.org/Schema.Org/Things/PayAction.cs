using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>An agent pays a price to a participant.</summary>
	public class PayAction: TradeAction
	{
		///<summary>A goal towards an action is taken. Can be concrete or abstract.</summary>
		public OneOfThese<MedicalDevicePurpose,Thing> Purpose {get; set;}
		///<summary>A sub property of participant. The participant who is at the receiving end of the action.</summary>
		public OneOfThese<Audience,ContactPoint,Organization,Person> Recipient {get; set;}
	}
}
