using Bam.Net.Configuration;
using Bam.Net.Data.Repositories;
using System;
using System.IO;
using System.Reflection;

namespace Bam.Net.Application
{
    [Serializable]
    public class GenerationSettings
    {
        public GenerationSettings()
        {
            Assembly = Assembly.GetExecutingAssembly();
            SchemaName = "BamNetDataSchema";
            FromNameSpace = "Bam.Net.Application.Data";
            ToNameSpace = "Bam.Net.Appliction.Data.Dao";
            UseInheritanceSchema = false;
            WriteSourceTo = Path.Combine(DataProvider.Current.GetAppDataDirectory(DefaultConfigurationApplicationNameProvider.Instance).FullName, "_gen", ToNameSpace);
        }

        public Assembly Assembly { get; set; }
        public string SchemaName { get; set; }
        public string FromNameSpace { get; set; }
        public string ToNameSpace { get; set; }

        public bool UseInheritanceSchema { get; set; }
        public string WriteSourceTo { get; set; }

        public DaoRepoGenerationConfig ToConfig()
        {
            return new DaoRepoGenerationConfig
            {
                TypeAssembly = Assembly.GetFilePath(),
                SchemaName = SchemaName,
                FromNameSpace = FromNameSpace,
                ToNameSpace = ToNameSpace,
                WriteSourceTo = WriteSourceTo,
                UseInheritanceSchema = UseInheritanceSchema,
                CheckForIds = true
            };
        }

        public static GenerationSettings FromConfig(string configPath)
        {
            if(configPath.EndsWith(".json", StringComparison.InvariantCultureIgnoreCase))
            {
                return FromConfig(configPath.FromJsonFile<DaoRepoGenerationConfig>());
            }
            else if(configPath.EndsWith(".yaml", StringComparison.InvariantCultureIgnoreCase) ||
                configPath.EndsWith(".yml", StringComparison.InvariantCultureIgnoreCase))
            {
                return FromConfig(configPath.FromYamlFile<DaoRepoGenerationConfig>());
            }
            throw new InvalidOperationException($"Unrecognized file extension: {configPath}");
        }

        public static GenerationSettings FromConfig(DaoRepoGenerationConfig config)
        {
            return new GenerationSettings
            {
                Assembly = Assembly.LoadFile(new FileInfo(config.TypeAssembly).FullName),
                SchemaName = config.SchemaName,
                FromNameSpace = config.FromNameSpace,
                ToNameSpace = config.ToNameSpace,
                WriteSourceTo = config.WriteSourceTo,
                UseInheritanceSchema = config.UseInheritanceSchema
            };
        }
    }
}
