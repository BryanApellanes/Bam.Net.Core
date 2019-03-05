using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A sub-grouping of food or drink items in a menu. E.g. courses (such as 'Dinner', 'Breakfast', etc.), specific type of dishes (such as 'Meat', 'Vegan', 'Drinks', etc.), or some other classification made by the menu provider.</summary>
	public class MenuSection: CreativeWork
	{
		///<summary>A food or drink item contained in a menu or menu section.</summary>
		public MenuItem HasMenuItem {get; set;}
		///<summary>A subgrouping of the menu (by dishes, course, serving time period, etc.).</summary>
		public MenuSection HasMenuSection {get; set;}
	}
}
