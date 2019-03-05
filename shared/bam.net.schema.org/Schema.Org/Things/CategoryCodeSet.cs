using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A set of Category Code values.</summary>
	public class CategoryCodeSet: DefinedTermSet
	{
		///<summary>A Category code contained in this code set.</summary>
		public CategoryCode HasCategoryCode {get; set;}
	}
}
