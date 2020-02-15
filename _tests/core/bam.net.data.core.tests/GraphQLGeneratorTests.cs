using System;
using System.Reflection;
using Bam.Net.Automation;
using Bam.Net.Data.GraphQL;
using Bam.Net.Testing;
using Bam.Net.Testing.Unit;

namespace Bam.Net.Data.Tests
{
    public class GraphQLGeneratorTests : CommandLineTestInterface
    {
        [UnitTest]
        public void GraphQLGeneratorWritesCodeThatCompiles()
        {
            GraphQLTypeGenerator generator = new GraphQLTypeGenerator(new GraphQLGenerationConfig()
            {
                SchemaName = "ApplicationRegistration",
                TypeAssembly = typeof(BamHome).Assembly.GetFilePath(),
                FromNameSpace = "Bam.Net.CoreServices.ApplicationRegistration.Data",
                ToNameSpace = "Bam.Net.CoreServices.ApplicationRegistration.Data.GraphQL",
                WriteSourceTo = "./.bam/_gen/graphqlTest"
            });

            Assembly assembly = generator.GenerateAssembly();
            Expect.IsNotNull(assembly);
            Type[] types = assembly.GetTypes();
            Expect.IsNotNull(types);
            Expect.IsTrue(types.Length > 0);
            foreach (Type type in types)
            {
                OutLineFormat("{0}", type.FullName);
            }
        }
    }
}