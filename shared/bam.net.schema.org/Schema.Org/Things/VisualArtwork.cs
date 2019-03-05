using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A work of art that is primarily visual in character.</summary>
	public class VisualArtwork: CreativeWork
	{
		///<summary>The number of copies when multiple copies of a piece of artwork are produced - e.g. for a limited edition of 20 prints, 'artEdition' refers to the total number of copies (in this example "20").</summary>
		public OneOfThese<Integer,Text> ArtEdition {get; set;}
		///<summary>The material used. (e.g. Oil, Watercolour, Acrylic, Linoprint, Marble, Cyanotype, Digital, Lithograph, DryPoint, Intaglio, Pastel, Woodcut, Pencil, Mixed Media, etc.)</summary>
		public OneOfThese<Text,Url> ArtMedium {get; set;}
		///<summary>e.g. Painting, Drawing, Sculpture, Print, Photograph, Assemblage, Collage, etc.</summary>
		public OneOfThese<Text,Url> Artform {get; set;}
		///<summary>The primary artist for a work    in a medium other than pencils or digital line art--for example, if the    primary artwork is done in watercolors or digital paints.</summary>
		public Person Artist {get; set;}
		///<summary>The supporting materials for the artwork, e.g. Canvas, Paper, Wood, Board, etc. Supersedes surface.</summary>
		public OneOfThese<Text,Url> ArtworkSurface {get; set;}
		///<summary>The individual who adds color to inked drawings.</summary>
		public Person Colorist {get; set;}
		///<summary>The depth of the item.</summary>
		public OneOfThese<Distance,QuantitativeValue> Depth {get; set;}
		///<summary>The height of the item.</summary>
		public OneOfThese<Distance,QuantitativeValue> Height {get; set;}
		///<summary>The individual who traces over the pencil drawings in ink after pencils are complete.</summary>
		public Person Inker {get; set;}
		///<summary>The individual who adds lettering, including speech balloons and sound effects, to artwork.</summary>
		public Person Letterer {get; set;}
		///<summary>The individual who draws the primary narrative artwork.</summary>
		public Person Penciler {get; set;}
		///<summary>The width of the item.</summary>
		public OneOfThese<Distance,QuantitativeValue> Width {get; set;}
	}
}
