using System;
using System.Collections.Generic;
using System.Reflection;
using Bam.Net.Data.Repositories;
using Bam.Net.Data.Schema;

namespace Bam.Net.Data.GraphQL
{
    public class GraphQLTypeGenerator : AssemblyGenerator
    {
        public GraphQLTypeGenerator()
        {
            TypeSchemaGenerator = new TypeSchemaGenerator();
            Types = new HashSet<Type>();
        }

        public HashSet<Type> Types { get; private set; }

        public void AddType(Type type)
        {
            Types.Add(type);
        }

        public void AddTypes(params Type[] types)
        {
            types.Each(type => AddType(type));
        }
        
        public TypeSchemaGenerator TypeSchemaGenerator { get; set; }
        
        public SchemaDefinition SchemaDefinition { get; set; }
        
        public override void WriteSource(string writeSourceDir)
        {
            throw new System.NotImplementedException();
        }

        public override Assembly Compile()
        {
            throw new System.NotImplementedException();
        }
    }
}