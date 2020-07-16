using Bam.Net.Application;
using Bam.Net.Data.Schema;
using Bam.Net.Data.Schema.Handlebars;
using Bam.Net.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bam.Net.Data.Repositories.Handlebars
{
    public class HandlebarsSchemaRepositoryGenerator: SchemaRepositoryGenerator
    {
        public HandlebarsSchemaRepositoryGenerator(DaoRepoGenerationConfig config, ILogger logger = null) : base(SchemaRepositoryGeneratorSettings.FromConfig(config), logger)
        {
            TemplateRenderer = new HandlebarsTemplateRenderer(new Presentation.Handlebars.HandlebarsEmbeddedResources(typeof(SchemaRepositoryGenerator).Assembly), new Presentation.Handlebars.HandlebarsDirectory(config.TemplatePath));

            Configure(config);
            Bam.Net.Handlebars.HandlebarsDirectory = new Presentation.Handlebars.HandlebarsDirectory(config.TemplatePath);
            Bam.Net.Handlebars.HandlebarsEmbeddedResources = new Presentation.Handlebars.HandlebarsEmbeddedResources(typeof(SchemaRepositoryGenerator).Assembly);
        }

        public override SchemaTypeModel GetSchemaTypeModel(Type t)
        {
            return HandlebarsSchemaTypeModel.FromType(t, DaoNamespace);
        }
    }
}
