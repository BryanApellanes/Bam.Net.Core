using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A single message from a sender to one or more organizations or people.</summary>
	public class Message: CreativeWork
	{
		///<summary>A sub property of recipient. The recipient blind copied on a message.</summary>
		public OneOfThese<ContactPoint,Organization,Person> BccRecipient {get; set;}
		///<summary>A sub property of recipient. The recipient copied on a message.</summary>
		public OneOfThese<ContactPoint,Organization,Person> CcRecipient {get; set;}
		///<summary>The date/time at which the message has been read by the recipient if a single recipient exists.</summary>
		public Bam.Net.Schema.Org.DataTypes.Date DateRead {get; set;}
		///<summary>The date/time the message was received if a single recipient exists.</summary>
		public Bam.Net.Schema.Org.DataTypes.Date DateReceived {get; set;}
		///<summary>The date/time at which the message was sent.</summary>
		public Bam.Net.Schema.Org.DataTypes.Date DateSent {get; set;}
		///<summary>A CreativeWork attached to the message.</summary>
		public CreativeWork MessageAttachment {get; set;}
		///<summary>A sub property of participant. The participant who is at the receiving end of the action.</summary>
		public OneOfThese<Audience,ContactPoint,Organization,Person> Recipient {get; set;}
		///<summary>A sub property of participant. The participant who is at the sending end of the action.</summary>
		public OneOfThese<Audience,Organization,Person> Sender {get; set;}
		///<summary>A sub property of recipient. The recipient who was directly sent the message.</summary>
		public OneOfThese<Audience,ContactPoint,Organization,Person> ToRecipient {get; set;}
	}
}
