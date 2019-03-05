using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A unique instance of a BroadcastService on a CableOrSatelliteService lineup.</summary>
	public class BroadcastChannel: Intangible
	{
		///<summary>The unique address by which the BroadcastService can be identified in a provider lineup. In US, this is typically a number.</summary>
		public Text BroadcastChannelId {get; set;}
		///<summary>The frequency used for over-the-air broadcasts. Numeric values or simple ranges e.g. 87-99. In addition a shortcut idiom is supported for frequences of AM and FM radio channels, e.g. "87 FM".</summary>
		public OneOfThese<BroadcastFrequencySpecification,Text> BroadcastFrequency {get; set;}
		///<summary>The type of service required to have access to the channel (e.g. Standard or Premium).</summary>
		public Text BroadcastServiceTier {get; set;}
		///<summary>Genre of the creative work, broadcast channel or group.</summary>
		public OneOfThese<Text,Url> Genre {get; set;}
		///<summary>The CableOrSatelliteService offering the channel.</summary>
		public CableOrSatelliteService InBroadcastLineup {get; set;}
		///<summary>The BroadcastService offered on this channel. Inverse property: hasBroadcastChannel.</summary>
		public BroadcastService ProvidesBroadcastService {get; set;}
	}
}
