using Bam.Net.Data.Schema;

namespace Bam.Shell.CodeGen
{
    public class SchemaExtractorInfo
    {
        public ISchemaExtractor SchemaExtractor { get; set; }
        public ExtractionTargetDbTypes DbType { get; set; }
    }
}