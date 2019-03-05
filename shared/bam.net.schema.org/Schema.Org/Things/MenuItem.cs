using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A food or drink item listed in a menu or menu section.</summary>
	public class MenuItem: Intangible
	{
		///<summary>Additional menu item(s) such as a side dish of salad or side order of fries that can be added to this menu item. Additionally it can be a menu section containing allowed add-on menu items for this menu item.</summary>
		public OneOfThese<MenuItem,MenuSection> MenuAddOn {get; set;}
		///<summary>Nutrition information about the recipe or menu item.</summary>
		public NutritionInformation Nutrition {get; set;}
		///<summary>An offer to provide this item—for example, an offer to sell a product, rent the DVD of a movie, perform a service, or give away tickets to an event.</summary>
		public Offer Offers {get; set;}
		///<summary>Indicates a dietary restriction or guideline for which this recipe or menu item is suitable, e.g. diabetic, halal etc.</summary>
		public RestrictedDiet SuitableForDiet {get; set;}
	}
}
