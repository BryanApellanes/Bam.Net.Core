using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A specific dosing schedule for a drug or supplement.</summary>
	public class DoseSchedule: MedicalIntangible
	{
		///<summary>The unit of the dose, e.g. 'mg'.</summary>
		public Text DoseUnit {get; set;}
		///<summary>The value of the dose, e.g. 500.</summary>
		public OneOfThese<Number,QualitativeValue> DoseValue {get; set;}
		///<summary>How often the dose is taken, e.g. 'daily'.</summary>
		public Text Frequency {get; set;}
		///<summary>Characteristics of the population for which this is intended, or which typically uses it, e.g. 'adults'.</summary>
		public Text TargetPopulation {get; set;}
	}
}
