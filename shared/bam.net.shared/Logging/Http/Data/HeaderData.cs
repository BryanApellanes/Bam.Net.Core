using Bam.Net.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Logging.Http.Data
{
    [Serializable]
    public class HeaderData: KeyedRepoData
    {
        [CompositeKey]
        public string Name { get; set; }
        [CompositeKey]
        public string Value { get; set; }
    }
}
