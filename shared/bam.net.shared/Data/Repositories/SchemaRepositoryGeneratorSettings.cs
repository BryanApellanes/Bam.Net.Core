using Bam.Net.Application;
using Bam.Net.Data.Repositories.Handlebars;
using Bam.Net.Data.Schema;
using Bam.Net.Data.Schema.Handlebars;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Bam.Net.Presentation.Handlebars;
using GraphQL;

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
        public GenerationConfig Config { get; set; }

        public static SchemaRepositoryGeneratorSettings FromConfig(GenerationConfig config)
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
