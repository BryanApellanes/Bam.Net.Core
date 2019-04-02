using System;
using Bam.Net.Application;
using Bam.Net.CommandLine;
using Bam.Net.Data.GraphQL;

namespace Bam.Shell.CodeGen
{
    public class GraphQLTypeProvider : CodeGenProvider
    {
        public override void Gen(Action<string> output = null, Action<string> error = null)
        {
            GraphQLTypeGenerator generator = new GraphQLTypeGenerator();
            GenerationConfig config = UtilityActions.GetGenerationConfig(o=> OutLineFormat(o, ConsoleColor.Blue));
            throw new NotImplementedException("finish this");
        }
    }
}