using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>An application programming interface accessible over Web/Internet technologies.</summary>
	public class WebAPI: Service
	{
		///<summary>Further documentation describing the Web API in more detail.</summary>
		public OneOfThese<CreativeWork,Url> Documentation {get; set;}
	}
}
