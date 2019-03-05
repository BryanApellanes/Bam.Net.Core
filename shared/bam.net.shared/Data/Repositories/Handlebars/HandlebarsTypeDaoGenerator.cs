using Bam.Net.Data.Schema;
using Bam.Net.Data.Schema.Handlebars;
using Bam.Net.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Data.Repositories.Handlebars
{
    public class HandlebarsTypeDaoGenerator: TypeDaoGenerator
    {
        public HandlebarsTypeDaoGenerator(TypeSchemaGenerator typeSchemaGenerator, ILogger logger = null) : base(new HandlebarsDaoCodeWriter(), new DaoTargetStreamResolver())
        {
            TypeSchemaGenerator = typeSchemaGenerator;
            WrapperGenerator = new HandlebarsWrapperGenerator();
        }
    }
}
