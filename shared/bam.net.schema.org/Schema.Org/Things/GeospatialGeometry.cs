using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>(Eventually to be defined as) a supertype of GeoShape designed to accommodate definitions from Geo-Spatial best practices.</summary>
	public class GeospatialGeometry: Intangible
	{
		///<summary>Represents a relationship between two geometries (or the places they represent), relating a containing geometry to a contained geometry. "a contains b iff no points of b lie in the exterior of a, and at least one point of the interior of b lies in the interior of a". As defined in DE-9IM.</summary>
		public OneOfThese<GeospatialGeometry,Place> GeospatiallyContains {get; set;}
		///<summary>Represents a relationship between two geometries (or the places they represent), relating a geometry to another that covers it. As defined in DE-9IM.</summary>
		public OneOfThese<GeospatialGeometry,Place> GeospatiallyCoveredBy {get; set;}
		///<summary>Represents a relationship between two geometries (or the places they represent), relating a covering geometry to a covered geometry. "Every point of b is a point of (the interior or boundary of) a". As defined in DE-9IM.</summary>
		public OneOfThese<GeospatialGeometry,Place> GeospatiallyCovers {get; set;}
		///<summary>Represents a relationship between two geometries (or the places they represent), relating a geometry to another that crosses it: "a crosses b: they have some but not all interior points in common, and the dimension of the intersection is less than that of at least one of them". As defined in DE-9IM.</summary>
		public OneOfThese<GeospatialGeometry,Place> GeospatiallyCrosses {get; set;}
		///<summary>Represents spatial relations in which two geometries (or the places they represent) are topologically disjoint: they have no point in common. They form a set of disconnected geometries." (a symmetric relationship, as defined in DE-9IM)</summary>
		public OneOfThese<GeospatialGeometry,Place> GeospatiallyDisjoint {get; set;}
		///<summary>Represents spatial relations in which two geometries (or the places they represent) are topologically equal, as defined in DE-9IM. "Two geometries are topologically equal if their interiors intersect and no part of the interior or boundary of one geometry intersects the exterior of the other" (a symmetric relationship)</summary>
		public OneOfThese<GeospatialGeometry,Place> GeospatiallyEquals {get; set;}
		///<summary>Represents spatial relations in which two geometries (or the places they represent) have at least one point in common. As defined in DE-9IM.</summary>
		public OneOfThese<GeospatialGeometry,Place> GeospatiallyIntersects {get; set;}
		///<summary>Represents a relationship between two geometries (or the places they represent), relating a geometry to another that geospatially overlaps it, i.e. they have some but not all points in common. As defined in DE-9IM.</summary>
		public OneOfThese<GeospatialGeometry,Place> GeospatiallyOverlaps {get; set;}
		///<summary>Represents spatial relations in which two geometries (or the places they represent) touch: they have at least one boundary point in common, but no interior points." (a symmetric relationship, as defined in DE-9IM )</summary>
		public OneOfThese<GeospatialGeometry,Place> GeospatiallyTouches {get; set;}
		///<summary>Represents a relationship between two geometries (or the places they represent), relating a geometry to one that contains it, i.e. it is inside (i.e. within) its interior. As defined in DE-9IM.</summary>
		public OneOfThese<GeospatialGeometry,Place> GeospatiallyWithin {get; set;}
	}
}
