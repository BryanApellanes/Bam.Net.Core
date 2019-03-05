using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Data.Schema;
using Bam.Net.ServiceProxy;

namespace Bam.Net.Data.Repositories
{
    public partial class SchemaRepositoryModel
    {
        public SchemaRepositoryModel()
        {
            BaseRepositoryType = "DaoRepository";
        }
        public string SchemaName { get; set; }
        public SchemaTypeModel[] Types { get; set; }
        public string BaseNamespace { get; set; }
        public string SchemaRepositoryNamespace { get; set; }
        public string BaseRepositoryType { get; set; }

    }
}
