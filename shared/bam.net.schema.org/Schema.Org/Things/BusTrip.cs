using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A trip on a commercial bus line.</summary>
	public class BusTrip: Trip
	{
		///<summary>The stop or station from which the bus arrives.</summary>
		public OneOfThese<BusStation,BusStop> ArrivalBusStop {get; set;}
		///<summary>The name of the bus (e.g. Bolt Express).</summary>
		public Text BusName {get; set;}
		///<summary>The unique identifier for the bus.</summary>
		public Text BusNumber {get; set;}
		///<summary>The stop or station from which the bus departs.</summary>
		public OneOfThese<BusStation,BusStop> DepartureBusStop {get; set;}
	}
}
