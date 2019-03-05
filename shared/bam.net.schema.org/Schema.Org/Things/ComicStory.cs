using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>The term "story" is any indivisible, re-printable    unit of a comic, including the interior stories, covers, and backmatter. Most    comics have at least two stories: a cover (ComicCoverArt) and an interior story.</summary>
	public class ComicStory: CreativeWork
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
	}
}
