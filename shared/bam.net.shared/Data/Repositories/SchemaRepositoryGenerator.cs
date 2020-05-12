using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Application;
using Bam.Net.Logging;

namespace Bam.Net.Data.Repositories
{
    /// <summary>
    /// A code and assembly generator used to generate schema
    /// specific dao repositories
    /// </summary>
    public partial class SchemaRepositoryGenerator: TypeDaoGenerator, IRepositorySourceGenerator
    {
        public SchemaRepositoryGenerator(SchemaRepositoryGeneratorSettings settings, ILogger logger = null)
        {
            if(logger != null)
            {
                Subscribe(logger);
            }

            DaoGenerator = new Schema.DaoGenerator(settings.DaoCodeWriter, settings.DaoTargetStreamResolver);
            WrapperGenerator = settings.WrapperGenerator;
            Configure(settings.Config);
        }

        /// <summary>
        /// Instantiate an instance of SchemaRepositoryGenerator that
        /// is used to generate a schema specific repository for the
        /// specified assembly for types in the specified 
        /// namespace.
        /// </summary>
        /// <param name="typeAssembly"></param>
        /// <param name="sourceNamespace"></param>
        /// <param name="logger"></param>
        public SchemaRepositoryGenerator(Assembly typeAssembly, string sourceNamespace, ILogger logger = null)
            : base(typeAssembly, sourceNamespace, logger)
        {
            SourceNamespace = sourceNamespace;
            BaseRepositoryType = "DaoRepository";
        }

        public ITemplateRenderer TemplateRenderer { get; set; }

        public DaoRepoGenerationConfig Config
        {
            get; private set;
        }

        public Assembly SourceAssembly { get; set; }

        public void Configure(DaoRepoGenerationConfig config)
        {
            if (config == null)
            {
                return;
            }
            Config = config;
            CheckIdField = config.CheckForIds;
            BaseRepositoryType = config.UseInheritanceSchema ? "DaoInheritanceRepository" : "DaoRepository";
            TargetNamespace = Config.FromNameSpace;
        }

        public virtual SchemaTypeModel GetSchemaTypeModel(Type t)
        {
            return SchemaTypeModel.FromType(t, DaoNamespace);
        }

        public void AddTypes()
        {
            EnsureConfigOrDie();
            SourceAssembly = Assembly.LoadFile(Config.TypeAssembly);
            Args.ThrowIfNull(SourceAssembly, $"Assembly not found {Config.TypeAssembly}", "SourceAssembly");
            AddTypes(SourceAssembly, Config.FromNameSpace);
        }

        public void AddTypes(Assembly typeAssembly, string sourceNamespace)
        {
            SourceNamespace = sourceNamespace;
            Args.ThrowIfNull(typeAssembly);
            AddTypes(typeAssembly.GetTypes().Where(t => t.Namespace != null && t.Namespace.Equals(sourceNamespace)));
        }

        public void GenerateRepositorySource()
        {
            AddTypes();
            Args.ThrowIf(Types.Length == 0, "No types were added");
            Args.ThrowIfNullOrEmpty(Config.WriteSourceTo, "WriteSourceTo");            
            GenerateRepositorySource(Config.WriteSourceTo, Config.SchemaName);
        }

        /// <summary>
        /// The namespace to analyze for types.
        /// </summary>
        public string SourceNamespace { get; set; }
        public string BaseRepositoryType { get; set; }
        public string SchemaRepositoryNamespace => $"{DaoNamespace}.Repository";

        public void GenerateSource()
        {
            EnsureConfigOrDie();
            GenerateSource(Config.WriteSourceTo);
        }

        public override void GenerateSource(string writeSourceTo)
        {
            base.GenerateSource(writeSourceTo);
            GenerateRepositorySource(writeSourceTo);
        }

        private void EnsureConfigOrDie()
        {
            if (Config == null)
            {
                Args.Throw<InvalidOperationException>("{0} not configured, first call Configure(GenerationConfig)", GetType().Name);
            }
        }
    }
}
