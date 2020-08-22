using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Bam.Net;
using Bam.Net.Application;
using Bam.Net.CommandLine;
using Bam.Net.Data.GraphQL;

namespace Bam.Shell.CodeGen
{
    public class GraphQLProvider : CodeGenProvider
    {
        public override void Generate(Action<string> output = null, Action<string> error = null)
        {
            // TODO: encapsulate GraphQLGenerationConfig provider logic; IGraphQLGenerationConfigProvider
            GraphQLGenerationConfig config = BamDbConsoleActions.GetGraphQLGenerationConfig(o=> OutLineFormat(o, ConsoleColor.Blue));
            Assembly assembly = Assembly.LoadFrom(config.TypeAssembly);
            if (assembly == null)
            {
                OutLineFormat("The specified type assembly wasn't found: {0}", ConsoleColor.Magenta, config.TypeAssembly);
                Exit(1);
            }

            GraphQLTypeGenerator generator = new GraphQLTypeGenerator(config);
            Assembly generated = generator.GenerateAssembly(out byte[] bytes);
            FileInfo file = new FileInfo($"{generator.AssemblyName}.dll");
            File.WriteAllBytes(file.FullName, bytes);
            OutLineFormat("Wrote file {0}", ConsoleColor.Blue, file.FullName);
        }
    }
}