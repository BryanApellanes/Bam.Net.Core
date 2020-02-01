using System;
using System.Collections.Generic;
using System.IO;
using Bam.Net.CommandLine;
using Bam.Net.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Application.Json
{
    [Serializable]
    public class ConsoleActions: CommandLineTestInterface
    {
        [ConsoleAction]
        public void LoadSchemaTest()
        {
            Dictionary<object, object> orgYaml = "/home/bryan/src/BamAppServices/_data/Vimly.Entity/v1/organization_v1.yaml".FromYamlFile() as Dictionary<object, object>;
            JSchema jSchema = JSchema.Parse(orgYaml.ToJson(), new FileSystemYamlJSchemaResolver("/home/bryan/src/BamAppServices/_data/Vimly.Entity/v1/"));
            OutLine(jSchema.ToJson(), ConsoleColor.Blue);
            OutLine(jSchema.ToString(), ConsoleColor.Cyan);
            
            // TODO: build dao schema 
            // read properties
            // read "javaType" property for class names
            // if type = "array" read items for type and setup foreign key
            // move this implementation to JsonSchemaYamlLoader
        }

    }
}