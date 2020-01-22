using System;
using System.IO;
using Bam.Net;
using Bam.Net.CommandLine;
using Bam.Net.Data.Schema;
using Bam.Net.Logging;

namespace Bam.Shell.CodeGen
{
    public class DaoProvider : CodeGenProvider
    {
        public override void RegisterArguments(string[] args)
        {
            base.RegisterArguments(args);
            AddValidArgument("output", false, true, "Dao: The directory path to output generated files to.");
            AddValidArgument("namespace", false,true, "Dao: The namespace to place generated classes into.");
            AddValidArgument("schema", false, false, "Dao: The path to the schema definition file.");
        }

        public override void Gen(Action<string> output = null, Action<string> error = null)
        {
            if (!Arguments.Contains("schema"))
            {
                error("Schema not specified, use /schema:[schemaDefinitionFilePath]");
                Exit(1);
            }
            SchemaDefinition schema = SchemaDefinition.Load(Arguments["schema"]);
            string writeTo = "./_gen/src";
            if (Arguments.Contains("output"))
            {
                writeTo = Arguments["output"];
                if(writeTo.StartsWith("~/"))
                {
                    writeTo = Path.Combine(BamPaths.UserHome, writeTo.TruncateFront(2));
                }
            }

            string srcDir = Path.Combine(writeTo, "src");
            
            DirectoryInfo outputDir = new DirectoryInfo(writeTo);

            output($"Generating Daos to {writeTo}...");
            string nameSpace = $"{schema.Name}.Dao";
            if (Arguments.Contains("namespace"))
            {
                nameSpace = Arguments["namespace"];
            }
            DaoGenerator daoGenerator = new DaoGenerator(nameSpace);
            daoGenerator.Generate(schema, writeTo);
            output("Generation complete.");
        }
    }
}