using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>Information about the engine of the vehicle. A vehicle can have multiple engines represented by multiple engine specification entities.</summary>
	public class EngineSpecification: StructuredValue
	{
		///<summary>The volume swept by all of the pistons inside the cylinders of an internal combustion engine in a single movement. Typical unit code(s): CMQ for cubic centimeter, LTR for liters, INQ for cubic inches* Note 1: You can link to information about how the given value has been determined using the valueReference property.* Note 2: You can use minValue and maxValue to indicate ranges.</summary>
		public QuantitativeValue EngineDisplacement {get; set;}
		///<summary>The power of the vehicle's engine.    Typical unit code(s): KWT for kilowatt, BHP for brake horsepower, N12 for metric horsepower (PS, with 1 PS = 735,49875 W)Note 1: There are many different ways of measuring an engine's power. For an overview, see  http://en.wikipedia.org/wiki/Horsepower#Enginepowertest_codes.Note 2: You can link to information about how the given value has been determined using the valueReference property.Note 3: You can use minValue and maxValue to indicate ranges.</summary>
		public QuantitativeValue EnginePower {get; set;}
		///<summary>The type of engine or engines powering the vehicle.</summary>
		public OneOfThese<QualitativeValue,Text,Url> EngineType {get; set;}
		///<summary>The type of fuel suitable for the engine or engines of the vehicle. If the vehicle has only one engine, this property can be attached directly to the vehicle.</summary>
		public OneOfThese<QualitativeValue,Text,Url> FuelType {get; set;}
		///<summary>The torque (turning force) of the vehicle's engine.Typical unit code(s): NU for newton metre (N m), F17 for pound-force per foot, or F48 for pound-force per inchNote 1: You can link to information about how the given value has been determined (e.g. reference RPM) using the valueReference property.Note 2: You can use minValue and maxValue to indicate ranges.</summary>
		public QuantitativeValue Torque {get; set;}
	}
}
