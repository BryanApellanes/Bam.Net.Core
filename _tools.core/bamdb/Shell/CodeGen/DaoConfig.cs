using System.IO;
using Bam.Net;

namespace Bam.Shell.CodeGen
{
    public class DaoConfig
    {
        public DaoConfig()
        {
            TemplatePath = Path.Combine(AppPaths.Data, "Templates");
        }
        
        public string TemplatePath { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string PostgresTableSchema { get; set; }
        
        public ExtractionTargetDbTypes DbType { get; set; }
    }
}