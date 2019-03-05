using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A recipe. For dietary restrictions covered by the recipe, a few common restrictions are enumerated via suitableForDiet. The keywords property can also be used to add more detail.</summary>
	public class Recipe: HowTo
	{
		///<summary>The time it takes to actually cook the dish, in ISO 8601 duration format.</summary>
		public Duration CookTime {get; set;}
		///<summary>The method of cooking, such as Frying, Steaming, ...</summary>
		public Text CookingMethod {get; set;}
		///<summary>Nutrition information about the recipe or menu item.</summary>
		public NutritionInformation Nutrition {get; set;}
		///<summary>The category of the recipe—for example, appetizer, entree, etc.</summary>
		public Text RecipeCategory {get; set;}
		///<summary>The cuisine of the recipe (for example, French or Ethiopian).</summary>
		public Text RecipeCuisine {get; set;}
		///<summary>A single ingredient used in the recipe, e.g. sugar, flour or garlic. Supersedes ingredients.</summary>
		public Text RecipeIngredient {get; set;}
		///<summary>A step in making the recipe, in the form of a single item (document, video, etc.) or an ordered list with HowToStep and/or HowToSection items.</summary>
		public OneOfThese<CreativeWork,ItemList,Text> RecipeInstructions {get; set;}
		///<summary>The quantity produced by the recipe (for example, number of people served, number of servings, etc).</summary>
		public OneOfThese<QuantitativeValue,Text> RecipeYield {get; set;}
		///<summary>Indicates a dietary restriction or guideline for which this recipe or menu item is suitable, e.g. diabetic, halal etc.</summary>
		public RestrictedDiet SuitableForDiet {get; set;}
	}
}
