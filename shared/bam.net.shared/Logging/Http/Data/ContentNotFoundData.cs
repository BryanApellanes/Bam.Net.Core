
using Bam.Net.Data.Repositories;

namespace Bam.Net.Logging.Http.Data
{
    public class ContentNotFoundData : KeyedRepoData
    {
        [CompositeKey]
        public string RequestId { get; set; }
        [CompositeKey]
        public string ResponderName { get; set; }
        
        public virtual ulong RequestDataId { get; set; }
        
        public virtual RequestData RequestData { get; set; }
        
        public string CheckedPaths { get; set; }
    }
}