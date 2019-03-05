using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>One of the sections into which a book is divided. A chapter usually has a section number or a name.</summary>
	public class Chapter: CreativeWork
	{
		///<summary>The page on which the work ends; for example "138" or "xvi".</summary>
		public OneOfThese<Integer,Text> PageEnd {get; set;}
		///<summary>The page on which the work starts; for example "135" or "xiii".</summary>
		public OneOfThese<Integer,Text> PageStart {get; set;}
		///<summary>Any description of pages that is not separated into pageStart and pageEnd; for example, "1-6, 9, 55" or "10-12, 46-49".</summary>
		public Text Pagination {get; set;}
	}
}
