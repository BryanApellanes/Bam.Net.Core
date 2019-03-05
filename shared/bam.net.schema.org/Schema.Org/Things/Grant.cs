using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A grant, typically financial or otherwise quantifiable, of resources. Typically a funder sponsors some MonetaryAmount to an Organization or Person,    sometimes not necessarily via a dedicated or long-lived Project, resulting in one or more outputs, or fundedItems. For financial sponsorship, indicate the funder of a MonetaryGrant. For non-financial support, indicate sponsor of Grants of resources (e.g. office space).Grants support  activities directed towards some agreed collective goals, often but not always organized as Projects. Long-lived projects are sometimes sponsored by a variety of grants over time, but it is also common for a project to be associated with a single grant.The amount of a Grant is represented using amount as a MonetaryAmount.</summary>
	public class Grant: Intangible
	{
		///<summary>Indicates an item funded or sponsored through a Grant.</summary>
		public Thing FundedItem {get; set;}
		///<summary>A person or organization that supports a thing through a pledge, promise, or financial contribution. e.g. a sponsor of a Medical Study or a corporate sponsor of an event.</summary>
		public OneOfThese<Organization,Person> Sponsor {get; set;}
	}
}
