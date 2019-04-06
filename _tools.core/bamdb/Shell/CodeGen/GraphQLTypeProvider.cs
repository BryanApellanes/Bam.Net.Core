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
            Assembly assembly = Assembly.LoadFrom(config.TypeAssembly);
            if (assembly == null)
            {
                OutLineFormat("The specified type assembly wasn't found: {0}", ConsoleColor.Magenta, config.TypeAssembly);
                Exit(1);
            }

            GraphQLTypeGenerator generator = new GraphQLTypeGenerator()
            {
                SourceDirectoryPath = config.WriteSourceTo,
                AssemblyName = config.ToNameSpace
            };
            generator.AddTypes(assembly.GetTypes().Where(type => RuntimeSettings.ClrTypeFilter(type) && type != null && type.Namespace != null && type.Namespace.Equals(config.FromNameSpace)).ToArray());
            Assembly generated = generator.GenerateAssembly();
            FileInfo file = new FileInfo($"{generator.AssemblyName}.dll");
            generated.ToBinaryFile(file.FullName);
            OutLineFormat("Wrote file {0}", ConsoleColor.Blue, file.FullName);
        }
    }
}