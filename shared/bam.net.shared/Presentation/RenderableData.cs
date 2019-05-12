using Bam.Net.Data.Repositories;
using Markdig.Syntax;

namespace Bam.Net.Presentation
{
    public class RenderableData : MarkdownObject
    {
        public RepoData Data { get; set; }
    }
}