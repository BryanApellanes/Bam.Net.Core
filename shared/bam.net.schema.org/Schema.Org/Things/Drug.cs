using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A chemical or biologic substance, used as a medical therapy, that has a physiological effect on an organism. Here the term drug is used interchangeably with the term medicine although clinical knowledge make a clear difference between them.</summary>
	public class Drug: Substance
	{
		///<summary>An active ingredient, typically chemical compounds and/or biologic substances.</summary>
		public Text ActiveIngredient {get; set;}
		///<summary>A route by which this drug may be administered, e.g. 'oral'.</summary>
		public Text AdministrationRoute {get; set;}
		///<summary>Any precaution, guidance, contraindication, etc. related to consumption of alcohol while taking this drug.</summary>
		public Text AlcoholWarning {get; set;}
		///<summary>An available dosage strength for the drug.</summary>
		public DrugStrength AvailableStrength {get; set;}
		///<summary>Any precaution, guidance, contraindication, etc. related to this drug's use by breastfeeding mothers.</summary>
		public Text BreastfeedingWarning {get; set;}
		///<summary>Description of the absorption and elimination of drugs, including their concentration (pharmacokinetics, pK) and biological effects (pharmacodynamics, pD). Supersedes clincalPharmacology.</summary>
		public Text ClinicalPharmacology {get; set;}
		///<summary>Cost per unit of the drug, as reported by the source being tagged.</summary>
		public DrugCost Cost {get; set;}
		///<summary>A dosage form in which this drug/supplement is available, e.g. 'tablet', 'suspension', 'injection'.</summary>
		public Text DosageForm {get; set;}
		///<summary>A dosing schedule for the drug for a given population, either observed, recommended, or maximum dose based on the type used.</summary>
		public DoseSchedule DoseSchedule {get; set;}
		///<summary>The class of drug this belongs to (e.g., statins).</summary>
		public DrugClass DrugClass {get; set;}
		///<summary>The unit in which the drug is measured, e.g. '5 mg tablet'.</summary>
		public Text DrugUnit {get; set;}
		///<summary>Any precaution, guidance, contraindication, etc. related to consumption of specific foods while taking this drug.</summary>
		public Text FoodWarning {get; set;}
		///<summary>The insurance plans that cover this drug.</summary>
		public HealthInsurancePlan IncludedInHealthInsurancePlan {get; set;}
		///<summary>Another drug that is known to interact with this drug in a way that impacts the effect of this drug or causes a risk to the patient. Note: disease interactions are typically captured as contraindications.</summary>
		public Drug InteractingDrug {get; set;}
		///<summary>True if the drug is available in a generic form (regardless of name).</summary>
		public Boolean IsAvailableGenerically {get; set;}
		///<summary>True if this item's name is a proprietary/brand name (vs. generic name).</summary>
		public Boolean IsProprietary {get; set;}
		///<summary>Link to the drug's label details.</summary>
		public Url LabelDetails {get; set;}
		///<summary>The drug or supplement's legal status, including any controlled substance schedules that apply.</summary>
		public OneOfThese<DrugLegalStatus,MedicalEnumeration,Text> LegalStatus {get; set;}
		///<summary>The manufacturer of the product.</summary>
		public Organization Manufacturer {get; set;}
		///<summary>Recommended intake of this supplement for a given population as defined by a specific recommending authority.</summary>
		public MaximumDoseSchedule MaximumIntake {get; set;}
		///<summary>The specific biochemical interaction through which this drug or supplement produces its pharmacological effect.</summary>
		public Text MechanismOfAction {get; set;}
		///<summary>The generic name of this drug or supplement.</summary>
		public Text NonProprietaryName {get; set;}
		///<summary>Any information related to overdose on a drug, including signs or symptoms, treatments, contact information for emergency response.</summary>
		public Text Overdosage {get; set;}
		///<summary>Pregnancy category of this drug.</summary>
		public DrugPregnancyCategory PregnancyCategory {get; set;}
		///<summary>Any precaution, guidance, contraindication, etc. related to this drug's use during pregnancy.</summary>
		public Text PregnancyWarning {get; set;}
		///<summary>Link to prescribing information for the drug.</summary>
		public Url PrescribingInfo {get; set;}
		///<summary>Indicates the status of drug prescription eg. local catalogs classifications or whether the drug is available by prescription or over-the-counter, etc.</summary>
		public OneOfThese<DrugPrescriptionStatus,Text> PrescriptionStatus {get; set;}
		///<summary>Proprietary name given to the diet plan, typically by its originator or creator.</summary>
		public Text ProprietaryName {get; set;}
		///<summary>Any other drug related to this one, for example commonly-prescribed alternatives.</summary>
		public Drug RelatedDrug {get; set;}
		///<summary>The RxCUI drug identifier from RXNORM.</summary>
		public Text Rxcui {get; set;}
		///<summary>Any FDA or other warnings about the drug (text or URL).</summary>
		public OneOfThese<Text,Url> Warning {get; set;}
	}
}
