using Bam.Net.Schema.Org.DataTypes;

namespace Bam.Net.Schema.Org.Things
{
	///<summary>A NewsArticle is an article whose content reports news, or provides background context and supporting materials for understanding the news.A more detailed overview of schema.org News markup is also available.</summary>
	public class NewsArticle: Article
	{
		///<summary>A dateline is a brief piece of text included in news articles that describes where and when the story was written or filed though the date is often omitted. Sometimes only a placename is provided.Structured representations of dateline-related information can also be expressed more explicitly using locationCreated (which represents where a work was created e.g. where a news report was written).  For location depicted or described in the content, use contentLocation.Dateline summaries are oriented more towards human readers than towards automated processing, and can vary substantially. Some examples: "BEIRUT, Lebanon, June 2.", "Paris, France", "December 19, 2017 11:43AM Reporting from Washington", "Beijing/Moscow", "QUEZON CITY, Philippines".</summary>
		public Text Dateline {get; set;}
		///<summary>The number of the column in which the NewsArticle appears in the print edition.</summary>
		public Text PrintColumn {get; set;}
		///<summary>The edition of the print product in which the NewsArticle appears.</summary>
		public Text PrintEdition {get; set;}
		///<summary>If this NewsArticle appears in print, this field indicates the name of the page on which the article is found. Please note that this field is intended for the exact page name (e.g. A5, B18).</summary>
		public Text PrintPage {get; set;}
		///<summary>If this NewsArticle appears in print, this field indicates the print section in which the article appeared.</summary>
		public Text PrintSection {get; set;}
	}
}
