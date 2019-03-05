using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A tourist attraction.  In principle any Thing can be a TouristAttraction, from a Mountain and LandmarksOrHistoricalBuildings to a LocalBusiness.  This Type can be used on its own to describe a general TouristAttraction, or be used as an additionalType to add tourist attraction properties to any other type.  (See examples below)</summary>
	public class TouristAttraction: Place
	{
		///<summary>A language someone may use with or at the item, service or place. Please use one of the language codes from the IETF BCP 47 standard. See also inLanguage</summary>
		public OneOfThese<Language,Text> AvailableLanguage {get; set;}
		///<summary>Attraction suitable for type(s) of tourist. eg. Children, visitors from a particular country, etc.</summary>
		public OneOfThese<Audience,Text> TouristType {get; set;}
	}
}
