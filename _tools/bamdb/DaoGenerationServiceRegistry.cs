using Bam.Net.Application;
using Bam.Net.CommandLine;
using Bam.Net.CoreServices;
using Bam.Net.Data.Repositories;
using Bam.Net.Data.Repositories.Handlebars;
using Bam.Net.Data.Schema;
using Bam.Net.Data.Schema.Handlebars;
using Bam.Net.Logging;
using Bam.Net.Presentation.Handlebars;
using Bam.Net.Services;
using Bam.Net.Testing;
using System;
using Bam.Net.Server;

namespace Bam.Net.Application
{
    [Serializable]
    public class DaoGenerationServiceRegistry : ApplicationServiceRegistry
    {
        [ServiceRegistryLoader]
        public static DaoGenerationServiceRegistry ForConfiguration(DaoRepoGenerationConfig config, ILogger logger = null)
        {
            DaoGenerationServiceRegistry daoRegistry = new DaoGenerationServiceRegistry();
            daoRegistry.CombineWith(Configure(appRegistry =>
            {
                appRegistry
                    .For<SchemaRepositoryGenerator>().Use(new HandlebarsSchemaRepositoryGenerator(config, logger))
                    .For<IDataDomainResolver>().Use<SchemaPrefixDataDomainResolver>();
            }));

            return daoRegistry;
        }
    }
}
