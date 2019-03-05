using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A blog.</summary>
	public class Blog: CreativeWork
	{
		///<summary>A posting that is part of this blog. Supersedes blogPosts.</summary>
		public BlogPosting BlogPost {get; set;}
		///<summary>The International Standard Serial Number (ISSN) that identifies this serial publication. You can repeat this property to identify different formats of, or the linking ISSN (ISSN-L) for, this serial publication.</summary>
		public Text Issn {get; set;}
	}
}
