using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>Entities that have a somewhat fixed, physical extension.</summary>
	public class Place: Thing
	{
		///<summary>A property-value pair representing an additional characteristics of the entitity, e.g. a product feature or another characteristic for which there is no matching property in schema.org.Note: Publishers should be aware that applications designed to use specific schema.org properties (e.g. http://schema.org/width, http://schema.org/color, http://schema.org/gtin13, ...) will typically expect such data to be provided using those properties, rather than using the generic property/value mechanism.</summary>
		public PropertyValue AdditionalProperty {get; set;}
		///<summary>Physical address of the item.</summary>
		public OneOfThese<PostalAddress,Text> Address {get; set;}
		///<summary>The overall rating, based on a collection of reviews or ratings, of the item.</summary>
		public AggregateRating AggregateRating {get; set;}
		///<summary>An amenity feature (e.g. a characteristic or service) of the Accommodation. This generic property does not make a statement about whether the feature is included in an offer for the main accommodation or available at extra costs.</summary>
		public LocationFeatureSpecification AmenityFeature {get; set;}
		///<summary>A short textual code (also called "store code") that uniquely identifies a place of business. The code is typically assigned by the parentOrganization and used in structured URLs.For example, in the URL http://www.starbucks.co.uk/store-locator/etc/detail/3047 the code "3047" is a branchCode for a particular branch.</summary>
		public Text BranchCode {get; set;}
		///<summary>The basic containment relation between a place and one that contains it. Supersedes containedIn. Inverse property: containsPlace.</summary>
		public Place ContainedInPlace {get; set;}
		///<summary>The basic containment relation between a place and another that it contains. Inverse property: containedInPlace.</summary>
		public Place ContainsPlace {get; set;}
		///<summary>Upcoming or past event associated with this place, organization, or action. Supersedes events.</summary>
		public Event Event {get; set;}
		///<summary>The fax number.</summary>
		public Text FaxNumber {get; set;}
		///<summary>The geo coordinates of the place.</summary>
		public OneOfThese<GeoCoordinates,GeoShape> Geo {get; set;}
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
		///<summary>The Global Location Number (GLN, sometimes also referred to as International Location Number or ILN) of the respective organization, person, or place. The GLN is a 13-digit number used to identify parties and physical locations.</summary>
		public Text GlobalLocationNumber {get; set;}
		///<summary>A URL to a map of the place. Supersedes map, maps.</summary>
		public OneOfThese<Map,Url> HasMap {get; set;}
		///<summary>A flag to signal that the item, event, or place is accessible for free. Supersedes free.</summary>
		public Boolean IsAccessibleForFree {get; set;}
		///<summary>The International Standard of Industrial Classification of All Economic Activities (ISIC), Revision 4 code for a particular organization, business person, or place.</summary>
		public Text IsicV4 {get; set;}
		///<summary>An associated logo.</summary>
		public OneOfThese<ImageObject,Url> Logo {get; set;}
		///<summary>The total number of individuals that may attend an event or venue.</summary>
		public Integer MaximumAttendeeCapacity {get; set;}
		///<summary>The opening hours of a certain place.</summary>
		public OpeningHoursSpecification OpeningHoursSpecification {get; set;}
		///<summary>A photograph of this place. Supersedes photos.</summary>
		public OneOfThese<ImageObject,Photograph> Photo {get; set;}
		///<summary>A flag to signal that the Place is open to public visitors.  If this property is omitted there is no assumed default boolean value</summary>
		public Boolean PublicAccess {get; set;}
		///<summary>A review of the item. Supersedes reviews.</summary>
		public Review Review {get; set;}
		///<summary>Indicates whether it is allowed to smoke in the place, e.g. in the restaurant, hotel or hotel room.</summary>
		public Boolean SmokingAllowed {get; set;}
		///<summary>The special opening hours of a certain place.Use this to explicitly override general opening hours brought in scope by openingHoursSpecification or openingHours.</summary>
		public OpeningHoursSpecification SpecialOpeningHoursSpecification {get; set;}
		///<summary>The telephone number.</summary>
		public Text Telephone {get; set;}
	}
}
