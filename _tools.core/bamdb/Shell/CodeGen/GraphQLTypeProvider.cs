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
    public class GraphQLTypeProvider : CodeGenProvider
    {
        public override void Gen(Action<string> output = null, Action<string> error = null)
        {
            GraphQLGenerationConfig config = UtilityActions.GetGraphQLGenerationConfig(o=> OutLineFormat(o, ConsoleColor.Blue));
            Assembly assembly = Assembly.LoadFile(config.TypeAssembly);
            if (assembly == null)
            {
                OutLineFormat("The specified type assembly wasn't found: {0}", ConsoleColor.Magenta, config.TypeAssembly);
                Exit(1);
            }

            GraphQLTypeGenerator generator = new GraphQLTypeGenerator()
            {
                SourceDirectoryPath = config.WriteSourceTo
            };
            generator.AddTypes(assembly.GetTypes().Where(type => type.Namespace.Equals(config.FromNameSpace)).ToArray());
            Assembly generated = generator.GenerateAssembly();
            FileInfo file = new FileInfo(generated.Location);
            File.Copy(file.FullName, Path.Combine(".", file.Name));
            OutLineFormat("Wrote file {0}", ConsoleColor.Blue, Path.Combine(".", file.Name));
        }
    }
}