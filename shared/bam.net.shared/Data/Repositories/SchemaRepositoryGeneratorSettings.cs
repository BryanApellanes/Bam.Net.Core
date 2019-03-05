using Bam.Net.Application;
using Bam.Net.Data.Repositories.Handlebars;
using Bam.Net.Data.Schema;
using Bam.Net.Data.Schema.Handlebars;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

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
            return new SchemaRepositoryGeneratorSettings
            (
                new HandlebarsDaoCodeWriter(new Presentation.Handlebars.HandlebarsDirectory(config.TemplatePath), new Presentation.Handlebars.HandlebarsEmbeddedResources(typeof(SchemaRepositoryGenerator).Assembly)),
                new DaoTargetStreamResolver(),
                new HandlebarsWrapperGenerator()
            )
            {
                Config = config
            };
        }
    }
}
