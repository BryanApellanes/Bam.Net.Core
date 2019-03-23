using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Bam.Net;
using Bam.Net.Application;
using Bam.Net.CommandLine;
using Bam.Net.Data.Dynamic;
using Bam.Net.Presentation.Handlebars;

namespace Bam.Shell.Models
{
    public class ModelProvider : ShellProvider
    {
        public override void List(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Add(Action<string> output = null, Action<string> error = null)
        {
            string modelArg = GetArgument("name", "Enter the name of the model to add");
            AppDataModel dataModel = ParseDataModelArgument(modelArg);
            DirectoryInfo projectParent = FindProjectParent(out FileInfo csprojFile);
            if (csprojFile == null)
            {
                OutLine("Can't find csproj file", ConsoleColor.Magenta);
                Exit(1);
            }
            WriteDataModelDefinition(csprojFile, dataModel);
            GenerateDataModels();
        }

        public override void Show(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Set(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Remove(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Run(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }
        
        [ArgZero("import", "Import data files from AppData (csv, json and yaml)")]
        public void ImportDataFiles()
        {
            DynamicDataManager mgr = new DynamicDataManager();
            mgr.ProcessDataFiles(AppData);
        }
        
        [ArgZero("gen", "src|bin|models|dbjs|repo|all: Generate a dynamic type assembly for json and yaml data")]
        public void Generate()
        {
            GenerationTargets target = Arguments["gen"].ToEnum<GenerationTargets>();
            switch (target)
            {
                case GenerationTargets.Invalid:
                    throw new InvalidOperationException("Invalid generation target specified");
                case GenerationTargets.src:
                    GenerateDynamicTypeSource();
                    break;
                case GenerationTargets.bin:
                    GenerateDynamicTypeAssemblies();
                    break;
                case GenerationTargets.models:
                    GenerateDataModels();
                    break;
                case GenerationTargets.dbjs:
                    GenerateDaoFromDbJsFiles();
                    break;
                case GenerationTargets.repo:
                    if (Arguments.Contains("config"))
                    {
                        GenerateSchemaRepository(Arguments["config"]);
                    }
                    else
                    {
                        GenerateSchemaRepository();
                    }
                    break;
                case GenerationTargets.all:
                default:
                    GenerateDynamicTypeSource();
                    GenerateDaoFromDbJsFiles();
                    GenerateDynamicTypeAssemblies();
                    GenerateSchemaRepository();
                    break;
            }
        }
        
        public const string AppDataFolderName = "AppData";
        public const string GenerationOutputFolderName = "_gen";
        
        static DirectoryInfo _appData;
        static object _appDataLock = new object();
        static DirectoryInfo AppData
        {
            get
            {
                return _appDataLock.DoubleCheckLock(ref _appData, () => new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, AppDataFolderName)));
            }
        }
        
        private void GenerateDataModels()
        {
            FileInfo csprojFile = FindProjectFile();
            DirectoryInfo projectParent = csprojFile.Directory;
            DirectoryInfo appModels = new DirectoryInfo(Path.Combine(projectParent.FullName, "AppModels"));
            DirectoryInfo appModelDefinitions = new DirectoryInfo(Path.Combine(appModels.FullName, "Definitions"));

            foreach(FileInfo yamlFile in appModelDefinitions.GetFiles("*.yaml"))
            {
                string dataModelName = Path.GetFileNameWithoutExtension(yamlFile.Name);
                RenderDataModel(csprojFile, dataModelName);
            }
        }

        private void RenderDataModel(FileInfo csprojFile, string dataModelName)
        {
            DirectoryInfo projectParent = csprojFile.Directory;
            DirectoryInfo appModels = new DirectoryInfo(Path.Combine(projectParent.FullName, "AppModels"));
            FileInfo modelCodeFile = new FileInfo(Path.Combine(appModels.FullName, $"{dataModelName}.cs"));

            HandlebarsDirectory handlebarsDirectory = GetHandlebarsDirectory();
            string appName = Path.GetFileNameWithoutExtension(csprojFile.Name);

            AppDataModel dataModel = ReadDataModelDefinition(csprojFile, dataModelName);

            handlebarsDirectory.Render("AppDataModel.cs", dataModel).SafeWriteToFile(modelCodeFile.FullName, true);
        }

        private void WriteDataModelDefinition(FileInfo csprojFile, AppDataModel appDataModel)
        {
            WriteDataModelDefinition(csprojFile, appDataModel.Name, appDataModel.Properties.ToArray());
        }

        private AppDataModel WriteDataModelDefinition(FileInfo csprojFile, string dataModelName, params AppDataPropertyModel[] properties)
        {
            AppDataModel model = ReadDataModelDefinition(csprojFile, dataModelName, out FileInfo dataModelFile);
            List<AppDataPropertyModel> props = new List<AppDataPropertyModel>();
            foreach(AppDataPropertyModel prop in properties)
            {
                props.Add(prop);
            }
            model.Properties = props.ToArray();
            model.ToYamlFile(dataModelFile);
            return model;
        }

        private AppDataModel ReadDataModelDefinition(FileInfo csprojFile, string dataModelName)
        {
            return ReadDataModelDefinition(csprojFile, dataModelName, out FileInfo ignore);
        }

        private AppDataModel ReadDataModelDefinition(FileInfo csprojFile, string dataModelName, out FileInfo modelFile)
        {
            DirectoryInfo projectParent = csprojFile.Directory;
            DirectoryInfo appModels = new DirectoryInfo(Path.Combine(projectParent.FullName, "AppModels"));
            DirectoryInfo appModelDefinitions = new DirectoryInfo(Path.Combine(appModels.FullName, "Definitions"));
            FileInfo file = new FileInfo(Path.Combine(appModelDefinitions.FullName, $"{dataModelName}.yaml"));
            string appName = Path.GetFileNameWithoutExtension(csprojFile.Name);
            AppDataModel model = new AppDataModel { BaseNamespace = appName, Name = dataModelName };
            modelFile = file;
            if (file.Exists)
            {
                model = file.FromYamlFile<AppDataModel>();
                if(model == null)
                {
                    OutLineFormat("{0} was empty or otherwise invalid", ConsoleColor.Yellow, file.FullName);
                    return new AppDataModel { BaseNamespace = appName, Name = dataModelName };
                }
            }
            return model;
        }

        private void GenerateDaoFromDbJsFiles()
        {
            //laotze.exe / root:[PATH-TO-DIRECTORY-CONTAINING-DBJS] /keep /s
            string writeTo = new DirectoryInfo(Path.Combine(AppData.FullName, "_gen", "src", "dao")).FullName;
            ProcessOutput output = $"laotze.exe /root:\"{AppData.FullName}\" /gen:\"{writeTo}\" /keep /s".Run(o => OutLine(o, ConsoleColor.DarkCyan), 100000);

            if (output.ExitCode != 0)
            {
                OutLineFormat("Dao generation from *.db.js files exited with code {0}: {1}", ConsoleColor.Yellow, output.ExitCode, output.StandardError.Substring(output.StandardError.Length - 300));
            }
        }

        private void GenerateSchemaRepository()
        {
            OutLineFormat("Generating Dao repository for AppModels", ConsoleColor.Cyan);
            FileInfo csprojFile = FindProjectFile();
            BamSettings settings = GetSettings();

            string schemaName = $"{Path.GetFileNameWithoutExtension(csprojFile.Name).Replace("_", "").Replace("-", "").Replace(".", "")}Schema";
            string dotnetTemp = new DirectoryInfo(Path.Combine(csprojFile.Directory.FullName, "AppData", "_gen", "dotnet")).FullName;

            ProcessStartInfo dotnet = settings.DotNetPath.ToStartInfo($"publish --output \"{dotnetTemp}\"");
            ProcessOutput dotnetOutput = dotnet.Run(o => OutLine(o, ConsoleColor.DarkGray), e => OutLine(e, ConsoleColor.Magenta), 600000);
            Assembly dotnetAssembly = Assembly.LoadFile(Path.Combine(dotnetTemp, $"{Path.GetFileNameWithoutExtension(csprojFile.Name)}.dll"));

            string fromNamespace = GetAppModelsNamespace(dotnetAssembly);
            if (string.IsNullOrEmpty(fromNamespace))
            {
                return;
            }
            string toNamespace = $"{fromNamespace}.GeneratedDao";

            GenerationSettings generationSettings = new GenerationSettings
            {
                Assembly = dotnetAssembly,
                SchemaName = schemaName,
                UseInheritanceSchema = false,
                FromNameSpace = fromNamespace,
                ToNameSpace = toNamespace,
                WriteSourceTo = Path.Combine(AppData.FullName, "_gen", "src", $"{schemaName}_Dao")
            };
            GenerateSchemaRepository(generationSettings);
        }

        private void GenerateSchemaRepository(string configPath)
        {
            GenerateSchemaRepository(GenerationSettings.FromConfig(configPath));
        }

        private void GenerateSchemaRepository(GenerationSettings generationSettings)
        {
            string bdbCommand = $"bdb.exe /generateSchemaRepository /typeAssembly:\"{generationSettings.Assembly.GetFilePath()}\" /schemaName:{generationSettings.SchemaName} /fromNameSpace:{generationSettings.FromNameSpace} /checkForIds:yes /useInhertianceSchema:{generationSettings.UseInheritanceSchema.ToString()} /writeSource:\"{generationSettings.WriteSourceTo}\"";
            ProcessOutput output = bdbCommand.Run(o => OutLine(o, ConsoleColor.DarkGreen), 100000);

            if (output.ExitCode != 0)
            {
                OutLineFormat("Schema generation exited with code {0}: {1}\r\n{2}", ConsoleColor.Yellow, output.ExitCode, output.StandardError, output.StandardOutput);
                Thread.Sleep(300);
            }
        }

        private void GenerateDynamicTypeSource()
        {
            OutLineFormat("Generating dynamic types from json ({0}) and yaml ({1}).", Path.Combine(AppData.FullName, "json"), Path.Combine(AppData.FullName, "yaml"));
            DynamicTypeManager dynamicTypeManager = new DynamicTypeManager();
            FileInfo csprojFile = FindProjectFile();
            if (csprojFile == null)
            {
                throw new InvalidOperationException("Couldn't find project file");
            }
            string source = dynamicTypeManager.GenerateSource(AppData, Path.GetFileNameWithoutExtension(csprojFile.Name));
            Expect.IsNotNullOrEmpty(source, "Source was not generated");

            OutLineFormat("Generated source: {0}", ConsoleColor.DarkCyan, source.Sha256());
        }

        private void GenerateDynamicTypeAssemblies()
        {
            DynamicTypeManager dynamicTypeManager = new DynamicTypeManager();
            Assembly assembly = dynamicTypeManager.GenerateAssembly(AppData);

            Expect.IsNotNull(assembly, "Assembly was not generated");
            Expect.IsGreaterThan(assembly.GetTypes().Length, 0, "No types were found in the generated assembly");

            foreach (Type type in assembly.GetTypes())
            {
                OutLineFormat("{0}.{1}", ConsoleColor.Cyan, type.Namespace, type.Name);
            }
        }



        protected string GetAppModelsNamespace(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.Namespace.EndsWith("AppModels"))
                {
                    return type.Namespace;
                }
            }
            OutLineFormat("No AppModels namespaces found", ConsoleColor.Yellow);
            return string.Empty;
        }

        private static AppDataModel ParseDataModelArgument(string modelArg)
        {
            string[] split = modelArg.DelimitSplit(",", true);
            string modelName = split[0];
            AppDataModel dataModel = new AppDataModel { Name = modelName };
            int num = 0;
            List<AppDataPropertyModel> props = new List<AppDataPropertyModel>();
            split.Rest(1, (modelProperty) =>
            {
                string[] parts = modelProperty.DelimitSplit(":", true);
                bool key = false;
                string type = "string";
                string name = $"_Property_{++num}";
                if (parts.Length == 2)
                {
                    type = parts[0];
                    name = parts[1];
                }
                else if (parts.Length == 3)
                {
                    type = parts[1];
                    name = parts[2];
                    string[] keyParts = parts[0].DelimitSplit("=", true);
                    if (keyParts.Length != 2)
                    {
                        OutLineFormat("Unrecognized key specification {0}: expected format key=[true|false].", ConsoleColor.Yellow, parts[0]);
                    }
                    else
                    {
                        key = keyParts[1].IsAffirmative();
                    }
                }
                props.Add(new AppDataPropertyModel
                {
                    Key = key,
                    Type = type,
                    Name = name
                });
            });
            dataModel.Properties = props.ToArray();
            return dataModel;
        }
        

    }
}