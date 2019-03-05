using Bam.Net.CommandLine;
using Bam.Net.CoreServices;
using Bam.Net.Messaging;
using Bam.Net.Testing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Automation.MSBuild;
using Bam.Net.Presentation.Handlebars;
using System.Threading;
using System.Reflection;
using Bam.Net.Data.Dynamic;
using Bam.Net.Data.Dynamic.Data;
using System.Diagnostics;

namespace Bam.Net.Application
{
    [Serializable]
    public class LifeCycleActions : CommandLineTestInterface
    {
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

        [ConsoleAction("config", "Write the default config file backing up the current file if it exists")]
        public void Config()
        {
            BamSettings settings = BamSettings.Load(true);
            if(!settings.IsValid(msgs => OutLine(msgs, ConsoleColor.Magenta)))
            {
                settings.Save(bak => OutLineFormat("Backed up existing file: {0}", ConsoleColor.DarkYellow, bak));
            }            
        }

        [ConsoleAction("init", "Add BamFramework to the current csproj")]
        public void Init()
        {
            // find the first csproj file by looking first in the current directory then going up
            // using the parent of the csproj as the root
            // - clone bam.js into wwwroot/bam.js
            // - write Startup.cs (backing up existing)
            // - write sample modules
            BamSettings settings = GetSettings();
            DirectoryInfo projectParent = FindProjectParent(out FileInfo csprojFile);
            if (csprojFile == null)
            {
                OutLine("Can't find csproj file", ConsoleColor.Magenta);

                Thread.Sleep(3000);
                Exit(1);
            }
            DirectoryInfo wwwroot = new DirectoryInfo(Path.Combine(projectParent.FullName, "wwwroot"));
            if (!wwwroot.Exists)
            {
                Warn("{0} doesn't exist, creating...", wwwroot.FullName);
                wwwroot.Create();
            }
            string bamJsPath = Path.Combine(wwwroot.FullName, "bam.js");
            if (!Directory.Exists(bamJsPath))
            {
                OutLineFormat("Cloning bam.js to {0}", ConsoleColor.Yellow, bamJsPath);
                ProcessStartInfo cloneCommand = settings.GitPath.ToStartInfo("clone https://github.com/BryanApellanes/bam.js.git wwwroot/bam.js");
                cloneCommand.Run(msg => OutLine(msg, ConsoleColor.DarkCyan));
            }

            WriteStartupCs(csprojFile);
            WriteBaseAppModules(csprojFile);
        }

        [ConsoleAction("import", "Import data files from AppData (csv, json and yaml)")]
        public void ImportDataFiles()
        {
            DynamicDataManager mgr = new DynamicDataManager();
            mgr.ProcessDataFiles(AppData);
        }

        [ConsoleAction("gen", "src|bin|models|dbjs|repo|all", "Generate a dynamic type assembly for json and yaml data")]
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

        [ConsoleAction("clean", "Clear all dynamic types and namespaces from the dynamic type manager")]
        public void CleanGeneratedTypes()
        {
            OutLine("Deleting ALL dynamic types from the local DynamicTypeManager", ConsoleColor.Yellow);
            DynamicTypeManager mgr = new DynamicTypeManager();
            mgr.DynamicTypeDataRepository.Query<DynamicTypePropertyDescriptor>(p => p.Id > 0).Each(p => mgr.DynamicTypeDataRepository.Delete(p));
            mgr.DynamicTypeDataRepository.Query<DynamicTypeDescriptor>(d => d.Id > 0).Each(d => mgr.DynamicTypeDataRepository.Delete(d));
            mgr.DynamicTypeDataRepository.Query<DynamicNamespaceDescriptor>(d => d.Id > 0).Each(d => mgr.DynamicTypeDataRepository.Delete(d));
            OutLine("Done", ConsoleColor.DarkYellow);
        }

        [ConsoleAction("addModel", "[modelName],[type1:propertyName1],[type2:propertyName2]...", "Add a model with the specified name and properties")]
        public void AddModel()
        {
            string modelArg = Arguments["addModel"];
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

        [ConsoleAction("addPage", "Add a page to the current BamFramework project")]
        public void AddPage()
        {
            string pageName = GetArgument("addPage", "Please enter the name of the page to create");
            if (string.IsNullOrEmpty(pageName))
            {
                OutLine("Page name not specified", ConsoleColor.Magenta);
                Exit(1);
            }

            // find the first csproj file by looking first in the current directory then going up
            // using the parent of the csproj as the root, add the files
            // - Pages/[pagePath].cshtml
            // - Pages/[pagePath].cshtml.cs
            // - wwwroot/bam.js/pages/[pagePath].js
            // - wwwroot/bam.js/configs/[pagePath]/webpack.config.js
            DirectoryInfo projectParent = FindProjectParent(out FileInfo csprojFile);
            if (csprojFile == null)
            {
                OutLine("Can't find csproj file", ConsoleColor.Magenta);
                Exit(1);
            }
            AddPage(csprojFile, pageName);
        }

        [ConsoleAction("pack", "WebPack each bam.js page found in wwwroot/bam.js/pages using corresponding configs found in wwwroot/bam.js/configs")]
        public void WebPack()
        {
            // find the first csproj file by looking first in the current directory then going up
            // using the parent of the csproj as the root
            // change directories into wwwroot/bam.js
            // for every webpack.config.js file in ./configs/ call
            // npx  webpack --config [configPath]

            string startDir = Environment.CurrentDirectory;

            DirectoryInfo projectParent = GetProjectParentDirectoryOrExit();
            BamSettings settings = GetSettings();
            DirectoryInfo wwwroot = new DirectoryInfo(Path.Combine(projectParent.FullName, "wwwroot"));
            if (!wwwroot.Exists)
            {
                OutLineFormat("{0} doesn't exist", ConsoleColor.Magenta, wwwroot.FullName);
                Exit(1);
            }
            DirectoryInfo bamJs = new DirectoryInfo(Path.Combine(wwwroot.FullName, "bam.js"));
            if (!bamJs.Exists)
            {
                OutLineFormat("{0} doesn't exist", ConsoleColor.Magenta, bamJs.FullName);
                Exit(1);
            }
            Environment.CurrentDirectory = bamJs.FullName;
            DirectoryInfo configs = new DirectoryInfo(Path.Combine(bamJs.FullName, "configs"));
            
            FileInfo[] webpackConfigs = configs.GetFiles("webpack.config.js", SearchOption.AllDirectories);
            foreach (FileInfo config in webpackConfigs)
            {
                string configPath = config.FullName.Replace(configs.Parent.FullName, "");
                if (configPath.StartsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    configPath = configPath.TruncateFront(1);
                }
                OutLineFormat("Packing {0}", ConsoleColor.Green, configPath);

                ProcessStartInfo webPackCommand = settings.NpxPath.ToCmdStartInfo($"webpack --config {configPath}");
                ProcessOutput output = webPackCommand.Run();
                OutLine(output.StandardOutput, ConsoleColor.DarkGreen);
            }
            if(webpackConfigs.Length == 0)
            {
                OutLineFormat("No webpack configs found in {0}", ConsoleColor.Yellow, configs.FullName);
            }
            Environment.CurrentDirectory = startDir;
        }

        [ConsoleAction("build", "Write a docker file and build a docker image.")]
        public void Build()
        {
            DirectoryInfo projectParent = GetProjectParentDirectoryOrExit(out FileInfo csprojFile);
            BamSettings settings = GetSettings();
            HandlebarsDirectory handlebars = GetHandlebarsDirectory();
            string projectName = Path.GetFileNameWithoutExtension(csprojFile.Name);
            string dockerFileContents = handlebars.Render("Dockerfile", new { AspNetCoreEnvironment = settings.Environment, ProjectName = projectName });
            string startDir = Environment.CurrentDirectory;
            Environment.CurrentDirectory = projectParent.FullName;
            string dockerFile = Path.Combine(".", "Dockerfile");
            dockerFileContents.SafeWriteToFile(dockerFile, true);
            ProcessStartInfo startInfo = settings.DockerPath.ToStartInfo($"tag {projectName} bamapps/containers:{projectName}");
            ProcessOutput tagOutput = startInfo.Run(msg => OutLine(msg, ConsoleColor.Blue));
            Environment.CurrentDirectory = startDir;
            if(tagOutput.ExitCode != 0)
            {
                OutLineFormat("docker tag command failed: {0}\r\n{1}", tagOutput.StandardOutput, tagOutput.StandardError);
                Exit(1);
            }
            ProcessOutput pushOutput = settings.DockerPath.ToStartInfo("push bamapps/containers:{projectName}").Run(msg => OutLine(msg, ConsoleColor.DarkCyan));
            if (tagOutput.ExitCode != 0)
            {
                OutLineFormat("docker push command failed: {0}\r\n{1}", tagOutput.StandardOutput, tagOutput.StandardError);
                Exit(1);
            }
        }

        [ConsoleAction("push", "Tag the docker image and push it to the bamapps docker registry.")]
        public void Push()
        {
            DirectoryInfo projectParent = GetProjectParentDirectoryOrExit(out FileInfo csprojFile);
            string projectName = Path.GetFileNameWithoutExtension(csprojFile.Name);
            BamSettings settings = GetSettings();
            string startDir = Environment.CurrentDirectory;
            settings.DockerPath.ToStartInfo($"tag {projectName} bamapps/images:{projectName}").Run(msg => OutLine(msg, ConsoleColor.Cyan));
            settings.DockerPath.ToStartInfo($"push bamapps/images:{projectName}").Run(msg=> OutLine(msg, ConsoleColor.DarkCyan));
            Environment.CurrentDirectory = startDir;
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

        private void AddPage(FileInfo csprojFile, string pageName)
        {
            DirectoryInfo projectParent = csprojFile.Directory;
            string appName = Path.GetFileNameWithoutExtension(csprojFile.Name);
            DirectoryInfo pagesDirectory = new DirectoryInfo(Path.Combine(projectParent.FullName, "Pages"));
            PageRenderModel pageRenderModel = new PageRenderModel { BaseNamespace = $"{appName}", PageName = pageName };

            HandlebarsDirectory handlebarsDirectory = GetHandlebarsDirectory();

            string csHtmlFilePath = Path.Combine(pagesDirectory.FullName, $"{pageName}.cshtml");            
            if (!File.Exists(csHtmlFilePath))
            {
                EnsureDirectoryExists(csHtmlFilePath);
                string pageContent = handlebarsDirectory.Render("Page.cshtml", pageRenderModel);
                OutLineFormat("Writing page file {0}", ConsoleColor.Cyan, csHtmlFilePath);
                pageContent.SafeWriteToFile(csHtmlFilePath, true);
            }

            string csHtmlcsFilePath = $"{csHtmlFilePath}.cs";
            if (!File.Exists(csHtmlcsFilePath))
            {
                EnsureDirectoryExists(csHtmlcsFilePath);
                string codeBehindContent = handlebarsDirectory.Render("Page.cshtml.cs", pageRenderModel);
                OutLineFormat("Writing code behind file {0}", ConsoleColor.DarkCyan, csHtmlcsFilePath);
                codeBehindContent.SafeWriteToFile(csHtmlcsFilePath, true);
            }

            AddWebPackConfig(csprojFile, pageName);
        }

        private void EnsureDirectoryExists(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
        }

        private void AddWebPackConfig(FileInfo csprojFile, string pageName)
        {
            DirectoryInfo wwwroot = new DirectoryInfo(Path.Combine(csprojFile.Directory.FullName, "wwwroot"));
            DirectoryInfo projectParent = csprojFile.Directory;
            string appName = Path.GetFileNameWithoutExtension(csprojFile.Name);
            string wwwrootPath = wwwroot.FullName;
            if (!wwwrootPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                wwwrootPath += Path.DirectorySeparatorChar.ToString();
            }
            PageRenderModel pageRenderModel = new PageRenderModel { BaseNamespace = $"{appName}", PageName = pageName, WwwRoot = wwwrootPath };

            HandlebarsDirectory handlebarsDirectory = GetHandlebarsDirectory();
            string pageJsPath = Path.Combine(wwwroot.FullName, "bam.js", "pages", $"{pageName}.js");
            string webPackConfigPath = Path.Combine(wwwroot.FullName, "bam.js", "configs", pageName, "webpack.config.js");
            if (!File.Exists(pageJsPath))
            {
                OutLineFormat("Writing page JavaScript file {0}", ConsoleColor.Blue, pageJsPath);
                handlebarsDirectory.Render("Page.js", pageRenderModel).SafeWriteToFile(pageJsPath, true);
            }
            if (!File.Exists(webPackConfigPath))
            {
                OutLineFormat("Writing web pack config file {0}", ConsoleColor.DarkBlue, webPackConfigPath);
                handlebarsDirectory.Render("Webpack.config.js", pageRenderModel).SafeWriteToFile(webPackConfigPath, true);
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
        
        private void WriteBaseAppModules(FileInfo csprojFile)
        {
            DirectoryInfo projectParent = csprojFile.Directory;
            DirectoryInfo appModules = new DirectoryInfo(Path.Combine(projectParent.FullName, "AppModules"));
            HandlebarsDirectory handlebarsDirectory = GetHandlebarsDirectory();
            string appName = Path.GetFileNameWithoutExtension(csprojFile.Name);

            AppModuleModel model = new AppModuleModel { BaseNamespace = appName, AppModuleName = appName };
            foreach(string moduleType in new string[] { "AppModule", "ScopedAppModule", "SingletonAppModule", "TransientAppModule" })
            {
                string moduleContent = handlebarsDirectory.Render($"{moduleType}.cs", model);
                if (string.IsNullOrEmpty(moduleContent))
                {
                    OutLineFormat("{0}: Template for {1} is empty", handlebarsDirectory.Directory.FullName, moduleType);
                }
                string filePath = Path.Combine(appModules.FullName, $"{appName}{moduleType}.cs");
                if (!File.Exists(filePath))
                {
                    moduleContent.SafeWriteToFile(filePath, true);
                    OutLineFormat("Wrote file {0}...", ConsoleColor.Green, filePath);
                }
            }            
        }

        private void WriteStartupCs(FileInfo csprojFile)
        {
            DirectoryInfo projectParent = csprojFile.Directory;
            FileInfo startupCs = new FileInfo(Path.Combine(projectParent.FullName, "Startup.cs"));
            if (startupCs.Exists)
            {
                string moveTo = startupCs.FullName.GetNextFileName();
                File.Move(startupCs.FullName, moveTo);
                OutLineFormat("Moved existing Startup.cs file to {0}", ConsoleColor.Yellow, moveTo);
            }

            HandlebarsDirectory handlebarsDirectory = GetHandlebarsDirectory();
            handlebarsDirectory.Render("Startup.cs", new { BaseNamespace = Path.GetFileNameWithoutExtension(csprojFile.Name) }).SafeWriteToFile(startupCs.FullName, true);
        }

        static HandlebarsDirectory _handlebarsDirectory;
        static object _handlebarsLock = new object();
        private static HandlebarsDirectory GetHandlebarsDirectory()
        {
            return _handlebarsLock.DoubleCheckLock(ref _handlebarsDirectory, () =>
            {
                DirectoryInfo bamDir = Assembly.GetExecutingAssembly().GetFileInfo().Directory;
                return new HandlebarsDirectory(Path.Combine(bamDir.FullName, "Templates"));
            });
        }

        private FileInfo FindProjectFile()
        {
            FindProjectParent(out FileInfo csprojFile);
            return csprojFile;
        }

        private DirectoryInfo FindProjectParent(out FileInfo csprojFile)
        {
            string startDir = Environment.CurrentDirectory;
            DirectoryInfo startDirInfo = new DirectoryInfo(startDir);
            DirectoryInfo projectParent = startDirInfo;
            FileInfo[] projectFiles = projectParent.GetFiles("*.csproj", SearchOption.TopDirectoryOnly);
            while (projectFiles.Length == 0)
            {
                if(projectParent.Parent != null)
                {
                    projectParent = projectParent.Parent;
                    projectFiles = projectParent.GetFiles("*.csrpoj", SearchOption.TopDirectoryOnly);
                }
                else
                {
                    break;
                }
            }
            csprojFile = null;
            if (projectFiles.Length > 0)
            {
                csprojFile = projectFiles[0];
            }

            if (projectFiles.Length > 1)
            {
                Warn("Multiple csproject files found, using {0}\r\n{1}", csprojFile.FullName, string.Join("\r\n\t", projectFiles.Select(p => p.FullName).ToArray()));
            }
            return projectParent;
        }

        private string GetAppModelsNamespace(Assembly assembly)
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
        
        private static BamSettings GetSettings()
        {
            BamSettings settings = BamSettings.Load();
            if (!settings.IsValid(msg => OutLine(msg, ConsoleColor.Red)))
            {
                Exit(1);
            }

            return settings;
        }

        private DirectoryInfo GetProjectParentDirectoryOrExit()
        {
            return GetProjectParentDirectoryOrExit(out FileInfo ignore);
        }

        private DirectoryInfo GetProjectParentDirectoryOrExit(out FileInfo csprojFile)
        {
            DirectoryInfo projectParent = FindProjectParent(out csprojFile);
            if (csprojFile == null)
            {
                OutLine("Can't find csproj file", ConsoleColor.Magenta);
                Exit(1);
            }

            return projectParent;
        }
    }
}
