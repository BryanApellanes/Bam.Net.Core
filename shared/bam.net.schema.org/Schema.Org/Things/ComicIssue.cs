using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>Individual comic issues are serially published as    part of a larger series. For the sake of consistency, even one-shot issues    belong to a series comprised of a single issue. All comic issues can be    uniquely identified by: the combination of the name and volume number of the    series to which the issue belongs; the issue number; and the variant    description of the issue (if any).</summary>
	public class ComicIssue: PublicationIssue
	{
		///<summary>The primary artist for a work    in a medium other than pencils or digital line art--for example, if the    primary artwork is done in watercolors or digital paints.</summary>
		public Person Artist {get; set;}
		///<summary>The individual who adds color to inked drawings.</summary>
		public Person Colorist {get; set;}
		///<summary>The individual who traces over the pencil drawings in ink after pencils are complete.</summary>
		public Person Inker {get; set;}
		///<summary>The individual who adds lettering, including speech balloons and sound effects, to artwork.</summary>
		public Person Letterer {get; set;}
		///<summary>The individual who draws the primary narrative artwork.</summary>
		public Person Penciler {get; set;}
		///<summary>A description of the variant cover    for the issue, if the issue is a variant printing. For example, "Bryan Hitch    Variant Cover" or "2nd Printing Variant".</summary>
		public Text VariantCover {get; set;}
	}
}
