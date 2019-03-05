using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A News/Media organization such as a newspaper or TV station.</summary>
	public class NewsMediaOrganization: Organization
	{
		///<summary>For a NewsMediaOrganization or other news-related Organization, a statement about public engagement activities (for news media, the newsroom’s), including involving the public - digitally or otherwise -- in coverage decisions, reporting and activities after publication.</summary>
		public OneOfThese<CreativeWork,Url> ActionableFeedbackPolicy {get; set;}
		///<summary>For an Organization (e.g. NewsMediaOrganization), a statement describing (in news media, the newsroom’s) disclosure and correction policy for errors.</summary>
		public OneOfThese<CreativeWork,Url> CorrectionsPolicy {get; set;}
		///<summary>Statement on diversity policy by an Organization e.g. a NewsMediaOrganization. For a NewsMediaOrganization, a statement describing the newsroom’s diversity policy on both staffing and sources, typically providing staffing data.</summary>
		public OneOfThese<CreativeWork,Url> DiversityPolicy {get; set;}
		///<summary>For an Organization (often but not necessarily a NewsMediaOrganization), a report on staffing diversity issues. In a news context this might be for example ASNE or RTDNA (US) reports, or self-reported.</summary>
		public OneOfThese<Article,Url> DiversityStaffingReport {get; set;}
		///<summary>Statement about ethics policy, e.g. of a NewsMediaOrganization regarding journalistic and publishing practices, or of a Restaurant, a page describing food source policies. In the case of a NewsMediaOrganization, an ethicsPolicy is typically a statement describing the personal, organizational, and corporate standards of behavior expected by the organization.</summary>
		public OneOfThese<CreativeWork,Url> EthicsPolicy {get; set;}
		///<summary>For a NewsMediaOrganization, a link to the masthead page or a page listing top editorial management.</summary>
		public OneOfThese<CreativeWork,Url> Masthead {get; set;}
		///<summary>For a NewsMediaOrganization, a statement on coverage priorities, including any public agenda or stance on issues.</summary>
		public OneOfThese<CreativeWork,Url> MissionCoveragePrioritiesPolicy {get; set;}
		///<summary>For a NewsMediaOrganization or other news-related Organization, a statement explaining when authors of articles are not named in bylines.</summary>
		public OneOfThese<CreativeWork,Url> NoBylinesPolicy {get; set;}
		///<summary>For an Organization (often but not necessarily a NewsMediaOrganization), a description of organizational ownership structure; funding and grants. In a news/media setting, this is with particular reference to editorial independence.   Note that the funder is also available and can be used to make basic funder information machine-readable.</summary>
		public OneOfThese<AboutPage,CreativeWork,Text,Url> OwnershipFundingInfo {get; set;}
		///<summary>For an Organization (typically a NewsMediaOrganization), a statement about policy on use of unnamed sources and the decision process required.</summary>
		public OneOfThese<CreativeWork,Url> UnnamedSourcesPolicy {get; set;}
		///<summary>Disclosure about verification and fact-checking processes for a NewsMediaOrganization or other fact-checking Organization.</summary>
		public OneOfThese<CreativeWork,Url> VerificationFactCheckingPolicy {get; set;}
	}
}
