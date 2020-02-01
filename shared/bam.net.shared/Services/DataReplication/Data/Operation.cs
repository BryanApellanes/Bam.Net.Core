/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.CoreServices.ApplicationRegistration.Data;
using Bam.Net.Data.Repositories;

namespace Bam.Net.Services.DataReplication.Data
{ 
    [Serializable]
    public abstract class Operation: AuditRepoData
    {
        public string TypeNamespace { get; set; }
        public string TypeName { get; set; }
        public string AssemblyPath { get; set; }

        protected internal string NamespaceQualifiedTypeName => $"{TypeNamespace}.{TypeName}";

        public abstract object Execute(IDistributedRepository repository);		

        public static TOperation For<TOperation>(Type type) where TOperation: Operation, new()
        {
            TOperation result = new TOperation()
            {
                TypeName = type.Name,
                TypeNamespace = type.Namespace,
                AssemblyPath = type?.Assembly?.GetFilePath()
            };
            
            return result;
        }
	}
}
