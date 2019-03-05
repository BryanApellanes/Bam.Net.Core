using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>The anatomical location at which two or more bones make contact.</summary>
	public class Joint: AnatomicalStructure
	{
		///<summary>The biomechanical properties of the bone.</summary>
		public Text BiomechnicalClass {get; set;}
		///<summary>The degree of mobility the joint allows.</summary>
		public OneOfThese<MedicalEntity,Text> FunctionalClass {get; set;}
		///<summary>The name given to how bone physically connects to each other.</summary>
		public Text StructuralClass {get; set;}
	}
}
