using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A dataset in downloadable form.</summary>
	public class DataDownload: MediaObject
	{
		///<summary>A technique or technology used in a Dataset (or DataDownload, DataCatalog),corresponding to the method used for measuring the corresponding variable(s) (described using variableMeasured). This is oriented towards scientific and scholarly dataset publication but may have broader applicability; it is not intended as a full representation of measurement, but rather as a high level summary for dataset discovery.For example, if variableMeasured is: molecule concentration, measurementTechnique could be: "mass spectrometry" or "nmr spectroscopy" or "colorimetry" or "immunofluorescence".If the variableMeasured is "depression rating", the measurementTechnique could be "Zung Scale" or "HAM-D" or "Beck Depression Inventory".If there are several variableMeasured properties recorded for some given data object, use a PropertyValue for each variableMeasured and attach the corresponding measurementTechnique.</summary>
		public OneOfThese<Text,Url> MeasurementTechnique {get; set;}
	}
}
