using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A statistical distribution of values.</summary>
	public class QuantitativeValueDistribution: QuantitativeValue
	{
		///<summary>The duration of the item (movie, audio recording, event, etc.) in ISO 8601 date format.</summary>
		public Duration Duration {get; set;}
		///<summary>The median value.</summary>
		public Number Median {get; set;}
		///<summary>The 10th percentile value.</summary>
		public Number Percentile10 {get; set;}
		///<summary>The 25th percentile value.</summary>
		public Number Percentile25 {get; set;}
		///<summary>The 75th percentile value.</summary>
		public Number Percentile75 {get; set;}
		///<summary>The 90th percentile value.</summary>
		public Number Percentile90 {get; set;}
	}
}
