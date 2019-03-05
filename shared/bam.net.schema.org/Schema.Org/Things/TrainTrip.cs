using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A trip on a commercial train line.</summary>
	public class TrainTrip: Trip
	{
		///<summary>The platform where the train arrives.</summary>
		public Text ArrivalPlatform {get; set;}
		///<summary>The station where the train trip ends.</summary>
		public TrainStation ArrivalStation {get; set;}
		///<summary>The platform from which the train departs.</summary>
		public Text DeparturePlatform {get; set;}
		///<summary>The station from which the train departs.</summary>
		public TrainStation DepartureStation {get; set;}
		///<summary>The name of the train (e.g. The Orient Express).</summary>
		public Text TrainName {get; set;}
		///<summary>The unique identifier for the train.</summary>
		public Text TrainNumber {get; set;}
	}
}
