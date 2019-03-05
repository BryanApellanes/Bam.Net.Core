using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A facility, often associated with a hospital or medical school, that is devoted to the specific diagnosis and/or healthcare. Previously limited to outpatients but with evolution it may be open to inpatients as well.</summary>
	public class MedicalClinic: MedicalOrganization
	{
		///<summary>A medical service available from this provider.</summary>
		public OneOfThese<MedicalProcedure,MedicalTest,MedicalTherapy> AvailableService {get; set;}
		///<summary>A medical specialty of the provider.</summary>
		public MedicalSpecialty MedicalSpecialty {get; set;}
	}
}
