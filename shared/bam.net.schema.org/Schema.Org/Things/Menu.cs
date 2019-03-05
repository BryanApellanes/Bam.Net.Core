using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A structured representation of food or drink items available from a FoodEstablishment.</summary>
	public class Menu: CreativeWork
	{
		///<summary>A food or drink item contained in a menu or menu section.</summary>
		public MenuItem HasMenuItem {get; set;}
		///<summary>A subgrouping of the menu (by dishes, course, serving time period, etc.).</summary>
		public MenuSection HasMenuSection {get; set;}
	}
}
