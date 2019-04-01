using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Bam.Net.Data.Repositories;
using Bam.Net.Data.Schema;

namespace Bam.Net.Data.GraphQL
{
    public class GraphQLTypeGenerator : AssemblyGenerator
    {
        public GraphQLTypeGenerator()
        {
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
        
        public override void WriteSource(string writeSourceDir)
        {
            Parallel.ForEach(Types, type => { });
        }

        public override Assembly Compile()
        {
            throw new System.NotImplementedException();
        }
    }
}