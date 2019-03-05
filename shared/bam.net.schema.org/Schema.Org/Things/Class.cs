using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A class, also often called a 'Type'; equivalent to rdfs:Class.</summary>
	public class Class: Intangible
	{
		///<summary>Relates a term (i.e. a property, class or enumeration) to one that supersedes it.</summary>
		public OneOfThese<Class,Enumeration,Property> SupersededBy {get; set;}
	}
}
