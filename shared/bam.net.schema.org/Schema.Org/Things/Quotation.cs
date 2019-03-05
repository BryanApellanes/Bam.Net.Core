using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A quotation. Often but not necessarily from some written work, attributable to a real world author and - if associated with a fictional character - to any fictional Person. Use isBasedOn to link to source/origin. The recordedIn property can be used to reference a Quotation from an Event.</summary>
	public class Quotation: CreativeWork
	{
		///<summary>The (e.g. fictional) character, Person or Organization to whom the quotation is attributed within the containing CreativeWork.</summary>
		public OneOfThese<Organization,Person> SpokenByCharacter {get; set;}
	}
}
