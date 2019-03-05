using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A car is a wheeled, self-powered motor vehicle used for transportation.</summary>
	public class Car: Vehicle
	{
		///<summary>The ACRISS Car Classification Code is a code used by many car rental companies, for classifying vehicles. ACRISS stands for Association of Car Rental Industry Systems and Standards.</summary>
		public Text AcrissCode {get; set;}
		///<summary>The permitted total weight of cargo and installations (e.g. a roof rack) on top of the vehicle.Typical unit code(s): KGM for kilogram, LBR for poundNote 1: You can indicate additional information in the name of the QuantitativeValue node.Note 2: You may also link to a QualitativeValue node that provides additional information using valueReferenceNote 3: Note that you can use minValue and maxValue to indicate ranges.</summary>
		public QuantitativeValue RoofLoad {get; set;}
	}
}
