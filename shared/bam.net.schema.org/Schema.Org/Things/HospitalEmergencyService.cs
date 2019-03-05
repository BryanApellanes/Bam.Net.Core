using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A hospital.</summary>
	public class HospitalEmergencyService: EmergencyService
	{
		///<summary>A medical service available from this provider.</summary>
		public OneOfThese<MedicalProcedure,MedicalTest,MedicalTherapy> AvailableService {get; set;}
		///<summary>A medical specialty of the provider.</summary>
		public MedicalSpecialty MedicalSpecialty {get; set;}
	}
}
