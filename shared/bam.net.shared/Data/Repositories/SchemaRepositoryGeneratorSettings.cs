using Bam.Net.Application;
using Bam.Net.Data.Repositories.Handlebars;
using Bam.Net.Data.Schema;
using Bam.Net.Data.Schema.Handlebars;
using Bam.Net.Presentation.Handlebars;

namespace Bam.Net.Data.Repositories
{
    public class SchemaRepositoryGeneratorSettings
    {
        public SchemaRepositoryGeneratorSettings(IDaoCodeWriter daoCodeWriter, IDaoTargetStreamResolver daoTargetStreamResolver, IWrapperGenerator wrapperGenerator)
        {
            DaoCodeWriter = daoCodeWriter;
            DaoTargetStreamResolver = daoTargetStreamResolver;
            WrapperGenerator = wrapperGenerator;
        }

        public IDaoCodeWriter DaoCodeWriter { get; set; }
        public IDaoTargetStreamResolver DaoTargetStreamResolver { get; set; }
        public IWrapperGenerator WrapperGenerator { get; set; }
        public DaoRepoGenerationConfig Config { get; set; }

        public static SchemaRepositoryGeneratorSettings FromConfig(DaoRepoGenerationConfig config)
        {
            HandlebarsDirectory handlebarsDirectory = new HandlebarsDirectory(config.TemplatePath);
            HandlebarsEmbeddedResources embeddedResources = new HandlebarsEmbeddedResources(typeof(SchemaRepositoryGenerator).Assembly);
            return new SchemaRepositoryGeneratorSettings
            (
                new HandlebarsDaoCodeWriter(handlebarsDirectory, embeddedResources),
                new DaoTargetStreamResolver(),
                new HandlebarsWrapperGenerator()
                    {HandlebarsDirectory = handlebarsDirectory, HandlebarsEmbeddedResources = embeddedResources}
            )
            {
                Config = config
            };
        }
    }
}
