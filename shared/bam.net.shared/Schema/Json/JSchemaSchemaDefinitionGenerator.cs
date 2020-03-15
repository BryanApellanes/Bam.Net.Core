using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bam.Net.Data.Schema;
using Bam.Net.Logging;
using Bam.Net.Schema.Json;
using CsQuery.ExtensionMethods;
using CsQuery.ExtensionMethods.Internal;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Application.Json
{
    /// <summary>
    /// A class used to generate a SchemaDefinition from one or more JSchemas
    /// </summary>
    public class JSchemaSchemaDefinitionGenerator
    {
        public JSchemaSchemaDefinitionGenerator(SchemaManager schemaManager, JSchemaClassManager jSchemaClassManager)
        {
            Args.ThrowIfNull(jSchemaClassManager, "jSchemaClassManager");
            SchemaManager = schemaManager;
            JSchemaClassManager = jSchemaClassManager;
            DiscoveredEnums = new Dictionary<string, HashSet<string>>();
            Logger = Log.Default;
        }
        
        private static SchemaManager _schemaManager;
        private static readonly object _schemaManagerLock = new object();
        public static SchemaManager DefaultSchemaManager
        {
            get { return _schemaManagerLock.DoubleCheckLock(ref _schemaManager, () => new SchemaManager()); }
            set => _schemaManager = value;
        }
        
        public ILogger Logger { get; set; }
        public SchemaManager SchemaManager { get; set; }
        public JSchemaClassManager JSchemaClassManager { get; set; }

        public Dictionary<string, HashSet<string>> DiscoveredEnums { get; set; }


    }
}