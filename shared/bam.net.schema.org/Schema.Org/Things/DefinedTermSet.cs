using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A set of defined terms for example a set of categories or a classification scheme, a glossary, dictionary or enumeration.</summary>
	public class DefinedTermSet: CreativeWork
	{
		///<summary>A Defined Term contained in this term set.</summary>
		public DefinedTerm HasDefinedTerm {get; set;}
	}
}
