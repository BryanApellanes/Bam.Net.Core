using System;
using System.Diagnostics;
using System.IO;
using Bam.Net;
using Bam.Net.Application;
using Bam.Net.CommandLine;
using Bam.Net.Presentation.Handlebars;

namespace Bam.Shell.Pages
{
    public class PageProvider : ShellProvider
    {
        public override void List(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Add(Action<string> output = null, Action<string> error = null)
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

        public override void Show(Action<string> output = null, Action<string> error = null)
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
        
        public override void Pack(Action<string> output = null, Action<string> error = null)
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
                ProcessOutput packOutput = webPackCommand.Run();
                OutLine(packOutput.StandardOutput, ConsoleColor.DarkGreen);
            }
            if(webpackConfigs.Length == 0)
            {
                OutLineFormat("No webpack configs found in {0}", ConsoleColor.Yellow, configs.FullName);
            }
            Environment.CurrentDirectory = startDir;
        }
    }
}