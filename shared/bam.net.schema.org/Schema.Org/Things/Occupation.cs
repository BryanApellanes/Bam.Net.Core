using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A profession, may involve prolonged training and/or a formal qualification.</summary>
	public class Occupation: Intangible
	{
		///<summary>Educational background needed for the position or Occupation.</summary>
		public Text EducationRequirements {get; set;}
		///<summary>A property describing the estimated salary for a job posting based on a variety of variables including, but not limited to industry, job title, and location. The estimated salary is usually computed by outside organizations and therefore the hiring organization is not bound to this estimated salary.</summary>
		public OneOfThese<MonetaryAmount,MonetaryAmountDistribution,Number,PriceSpecification> EstimatedSalary {get; set;}
		///<summary>Description of skills and experience needed for the position or Occupation.</summary>
		public Text ExperienceRequirements {get; set;}
		///<summary>The region/country for which this occupational description is appropriate. Note that educational requirements and qualifications can vary between jurisdictions.</summary>
		public AdministrativeArea OccupationLocation {get; set;}
		///<summary>Category or categories describing the job. Use BLS O*NET-SOC taxonomy: http://www.onetcenter.org/taxonomy.html. Ideally includes textual label and formal code, with the property repeated for each applicable value.</summary>
		public Text OccupationalCategory {get; set;}
		///<summary>Specific qualifications required for this role or Occupation.</summary>
		public Text Qualifications {get; set;}
		///<summary>Responsibilities associated with this role or Occupation.</summary>
		public Text Responsibilities {get; set;}
		///<summary>Skills required to fulfill this role.</summary>
		public Text Skills {get; set;}
	}
}
