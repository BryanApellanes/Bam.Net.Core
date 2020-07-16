using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Bam.Net.Configuration;
using Bam.Net.Data.Repositories;

namespace Bam.Net.Application
{
    /// <summary>
    /// Configuration for generating a dao repository.
    /// </summary>
    public class DaoRepoGenerationConfig
    {
        public DaoRepoGenerationConfig()
        {
            CheckForIds = true;
            TemplatePath = Path.Combine(AppPaths.Data, "Templates");
            WriteSourceTo = "Generated_Dao";
            TypeAssembly = GetType().Assembly.GetFileInfo().FullName;
        }

        public string TemplatePath { get; set; }
        
        [CompositeKey]
        public string TypeAssembly { get; set; }
        
        [CompositeKey]
        public string SchemaName { get; set; }

        /// <summary>
        /// Gets or sets from name space to find types to generate dao types and wrappers for.
        /// </summary>
        /// <value>
        /// From name space.
        /// </value>
        [CompositeKey]
        public string FromNameSpace { get; set; }
        
        [CompositeKey]
        public string ToNameSpace { get; set; }
        public string WriteSourceTo { get; set; }

        /// <summary>
        /// Check the specified data classes for Id properties
        /// </summary>
        public bool CheckForIds { get; set; }

        /// <summary>
        /// If yes the when generating a Repository, it will inherit from DaoInheritanceRepository otherwise DaoRepository
        /// </summary>
        public bool UseInheritanceSchema { get; set; }
    }
}
