using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A thesis or dissertation document submitted in support of candidature for an academic degree or professional qualification.</summary>
	public class Thesis: CreativeWork
	{
		///<summary>Qualification, candidature, degree, application that Thesis supports.</summary>
		public Text InSupportOf {get; set;}
	}
}
