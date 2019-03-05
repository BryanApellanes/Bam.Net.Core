using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>Any medical imaging modality typically used for diagnostic purposes.</summary>
	public class ImagingTest: MedicalTest
	{
		///<summary>Imaging technique used.</summary>
		public MedicalImagingTechnique ImagingTechnique {get; set;}
	}
}
