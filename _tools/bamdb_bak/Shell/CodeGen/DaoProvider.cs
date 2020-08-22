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

        public override void Generate(Action<string> output = null, Action<string> error = null)
        {
            if (!Arguments.Contains("schema"))
            {
                error("Schema not specified, use /schema:[schemaDefinitionFilePath]");
                Exit(1);
            }

            string schemaPath = Arguments["schema"];
            if (schemaPath.StartsWith("~/"))
            {
                schemaPath = Path.Combine(BamHome.UserHome, schemaPath.TruncateFront(2));
            }
                
            SchemaDefinition schema = SchemaDefinition.Load(schemaPath);
            SchemaNameMap nameMap = null;
            if (File.Exists($"{schema.File}.map.fixed"))
            {
                nameMap = SchemaNameMap.Load($"{schema.File}.map.fixed");
            }
            string writeTo = "./_gen/src";
            if (Arguments.Contains("output"))
            {
                writeTo = Arguments["output"];
                if(writeTo.StartsWith("~/"))
                {
                    writeTo = Path.Combine(BamHome.UserHome, writeTo.TruncateFront(2));
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
            if (nameMap != null)
            {
                MappedSchemaDefinition mappedSchemaDefinition = new MappedSchemaDefinition(schema, nameMap);
                schema = mappedSchemaDefinition.MapSchemaClassAndPropertyNames();
            }
            daoGenerator.Generate(schema, writeTo);
            output("Generation complete.");
        }
    }
}