using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A Category Code.</summary>
	public class CategoryCode: DefinedTerm
	{
		///<summary>A short textual code that uniquely identifies the value.</summary>
		public Text CodeValue {get; set;}
		///<summary>A CategoryCodeSet that contains this catagory code.</summary>
		public OneOfThese<CategoryCodeSet,Url> InCodeSet {get; set;}
	}
}
