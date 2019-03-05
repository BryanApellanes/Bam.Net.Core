using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A code for a medical entity.</summary>
	public class MedicalCodeCategoryCode: CategoryCode
	{
		///<summary>A short textual code that uniquely identifies the value.</summary>
		public Text CodeValue {get; set;}
		///<summary>The coding system, e.g. 'ICD-10'.</summary>
		public Text CodingSystem {get; set;}
	}
}
