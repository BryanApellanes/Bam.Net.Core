using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>Fitness-related activity designed for a specific health-related purpose, including defined exercise routines as well as activity prescribed by a clinician.</summary>
	public class ExercisePlanPhysicalActivity: PhysicalActivity
	{
		///<summary>Length of time to engage in the activity.</summary>
		public OneOfThese<Duration,QualitativeValue> ActivityDuration {get; set;}
		///<summary>How often one should engage in the activity.</summary>
		public OneOfThese<QualitativeValue,Text> ActivityFrequency {get; set;}
		///<summary>Any additional component of the exercise prescription that may need to be articulated to the patient. This may include the order of exercises, the number of repetitions of movement, quantitative distance, progressions over time, etc.</summary>
		public Text AdditionalVariable {get; set;}
		///<summary>Type(s) of exercise or activity, such as strength training, flexibility training, aerobics, cardiac rehabilitation, etc.</summary>
		public Text ExerciseType {get; set;}
		///<summary>Quantitative measure gauging the degree of force involved in the exercise, for example, heartbeats per minute. May include the velocity of the movement.</summary>
		public OneOfThese<QualitativeValue,Text> Intensity {get; set;}
		///<summary>Number of times one should repeat the activity.</summary>
		public OneOfThese<Number,QualitativeValue> Repetitions {get; set;}
		///<summary>How often one should break from the activity.</summary>
		public OneOfThese<QualitativeValue,Text> RestPeriods {get; set;}
		///<summary>Quantitative measure of the physiologic output of the exercise; also referred to as energy expenditure.</summary>
		public OneOfThese<Energy,QualitativeValue> Workload {get; set;}
	}
}
