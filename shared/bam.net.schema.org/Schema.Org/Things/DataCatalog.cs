using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A collection of datasets.</summary>
	public class DataCatalog: CreativeWork
	{
		///<summary>A dataset contained in this catalog. Inverse property: includedInDataCatalog.</summary>
		public Dataset Dataset {get; set;}
		///<summary>A technique or technology used in a Dataset (or DataDownload, DataCatalog),corresponding to the method used for measuring the corresponding variable(s) (described using variableMeasured). This is oriented towards scientific and scholarly dataset publication but may have broader applicability; it is not intended as a full representation of measurement, but rather as a high level summary for dataset discovery.For example, if variableMeasured is: molecule concentration, measurementTechnique could be: "mass spectrometry" or "nmr spectroscopy" or "colorimetry" or "immunofluorescence".If the variableMeasured is "depression rating", the measurementTechnique could be "Zung Scale" or "HAM-D" or "Beck Depression Inventory".If there are several variableMeasured properties recorded for some given data object, use a PropertyValue for each variableMeasured and attach the corresponding measurementTechnique.</summary>
		public OneOfThese<Text,Url> MeasurementTechnique {get; set;}
	}
}
