using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A body of structured information describing some topic(s) of interest.</summary>
	public class Dataset: CreativeWork
	{
		///<summary>A downloadable form of this dataset, at a specific location, in a specific format.</summary>
		public DataDownload Distribution {get; set;}
		///<summary>A data catalog which contains this dataset. Supersedes catalog, includedDataCatalog. Inverse property: dataset.</summary>
		public DataCatalog IncludedInDataCatalog {get; set;}
		///<summary>The International Standard Serial Number (ISSN) that identifies this serial publication. You can repeat this property to identify different formats of, or the linking ISSN (ISSN-L) for, this serial publication.</summary>
		public Text Issn {get; set;}
		///<summary>A technique or technology used in a Dataset (or DataDownload, DataCatalog),corresponding to the method used for measuring the corresponding variable(s) (described using variableMeasured). This is oriented towards scientific and scholarly dataset publication but may have broader applicability; it is not intended as a full representation of measurement, but rather as a high level summary for dataset discovery.For example, if variableMeasured is: molecule concentration, measurementTechnique could be: "mass spectrometry" or "nmr spectroscopy" or "colorimetry" or "immunofluorescence".If the variableMeasured is "depression rating", the measurementTechnique could be "Zung Scale" or "HAM-D" or "Beck Depression Inventory".If there are several variableMeasured properties recorded for some given data object, use a PropertyValue for each variableMeasured and attach the corresponding measurementTechnique.</summary>
		public OneOfThese<Text,Url> MeasurementTechnique {get; set;}
		///<summary>The variableMeasured property can indicate (repeated as necessary) the  variables that are measured in some dataset, either described as text or as pairs of identifier and description using PropertyValue.</summary>
		public OneOfThese<PropertyValue,Text> VariableMeasured {get; set;}
	}
}
