using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Bam.Net;
using Bam.Net.Application;
using Bam.Net.CommandLine;
using Bam.Net.Configuration;
using Bam.Net.Presentation.Handlebars;
using Bam.Net.Testing;

namespace Bam.Shell
{
    public class Init : CommandLineTestInterface
    {
        private static Dictionary<AppKind, Action> _initActions;
        static Init()
        {
            _initActions = new Dictionary<AppKind, Action>()
            {
                {AppKind.Bam, BamInit},
                {AppKind.Dao, DaoInit},
                {AppKind.Repo, RepoInit},
                {AppKind.Rest, RestInit},
                {AppKind.Razor, AspNetRazorInit},
            };
        }

        public static void BamInit()
        {
            // Read BAM_HOME environment variable
            string applicationName = EnvironmentApplicationNameProvider.Instance.GetApplicationName();
            Message.PrintLine("ApplicationName = {0}", ConsoleColor.DarkBlue, applicationName);

            string bamHome = BamEnvironmentVariables.Home();
            OutLineFormat("BAM_HOME = {0}", ConsoleColor.DarkBlue, bamHome);
            // create a directory in {{bamHome}}/content/apps/{{applicationName}}
            DirectoryInfo appHome = new DirectoryInfo(Path.Combine(bamHome, "content", "apps", applicationName));
            if (!appHome.Exists)
            {
                appHome.Create();
            }
            // create
            // - pages/
            DirectoryInfo pagesDir = new DirectoryInfo(Path.Combine(appHome.FullName, "pages"));
            if (!pagesDir.Exists)
            {
                pagesDir.Create();
            }
            
            //    home.html
            //    about.html
            //    contact.html
            //    tests.html
            
            // TODO: finish this
            throw new NotImplementedException();
            
            DirectoryInfo docsDir = new DirectoryInfo(Path.Combine(appHome.FullName, "docs"));
            if (!docsDir.Exists)
            {
                docsDir.Create();
            }
            //    docs/
            //        intro.md
            //        books.md
            //        data.md
            //        models.md
            //        viewModels.md
            // - data/
            // - data/dao/
            // - data/repo/
            // - data/json/
            // - data/yaml/
            // - data/csv/
            // - models/
            // - viewModels/
        }

        
        public static void RepoInit()
        {
            // bdb init
            // maybe write a repo project based on the bdb server
        }

        public static void DaoInit()
        {
            // bdb init
            // write a dao project based on the bdb server
        }

        public static void RestInit()
        {
            // dbd init
            // write a rest project based on the bdb server
        }
        
        public static void AspNetRazorInit()
        {
            // find the first csproj file by looking first in the current directory then going up
            // using the parent of the csproj as the root
            // - clone bam.js into wwwroot/bam.js
            // - write Startup.cs (backing up existing)
            // - write sample modules
            BamSettings settings = ShellProvider.GetSettings();
            DirectoryInfo projectParent = ShellProvider.FindProjectParent(out FileInfo csprojFile);
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
                ProcessStartInfo cloneCommand =
                    settings.GitPath.ToStartInfo("clone https://github.com/BryanApellanes/bam.js.git wwwroot/bam.js");
                cloneCommand.Run(msg => OutLine(msg, ConsoleColor.DarkCyan));
            }

            WriteStartupCs(csprojFile);
            WriteBaseAppModules(csprojFile);
        }  
        
        private static void WriteBaseAppModules(FileInfo csprojFile)
        {
            DirectoryInfo projectParent = csprojFile.Directory;
            DirectoryInfo appModules = new DirectoryInfo(Path.Combine(projectParent.FullName, "AppModules"));
            HandlebarsDirectory handlebarsDirectory = ShellProvider.GetHandlebarsDirectory();
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

        private static void WriteStartupCs(FileInfo csprojFile)
        {
            DirectoryInfo projectParent = csprojFile.Directory;
            FileInfo startupCs = new FileInfo(Path.Combine(projectParent.FullName, "Startup.cs"));
            if (startupCs.Exists)
            {
                string moveTo = startupCs.FullName.GetNextFileName();
                File.Move(startupCs.FullName, moveTo);
                OutLineFormat("Moved existing Startup.cs file to {0}", ConsoleColor.Yellow, moveTo);
            }

            HandlebarsDirectory handlebarsDirectory = ShellProvider.GetHandlebarsDirectory();
            handlebarsDirectory.Render("Startup.cs", new { BaseNamespace = Path.GetFileNameWithoutExtension(csprojFile.Name) }).SafeWriteToFile(startupCs.FullName, true);
        }
    }
}