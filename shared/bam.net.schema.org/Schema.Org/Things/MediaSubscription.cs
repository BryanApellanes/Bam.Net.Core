using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A subscription which allows a user to access media including audio, video, books, etc.</summary>
	public class MediaSubscription: Intangible
	{
		///<summary>The Organization responsible for authenticating the user's subscription. For example, many media apps require a cable/satellite provider to authenticate your subscription before playing media.</summary>
		public Organization Authenticator {get; set;}
		///<summary>An Offer which must be accepted before the user can perform the Action. For example, the user may need to buy a movie before being able to watch it.</summary>
		public Offer ExpectsAcceptanceOf {get; set;}
	}
}
