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
using Bam.Net;
using Bam.Net.Application;
using Bam.Net.Automation;
using Bam.Net.Data.Repositories;

namespace Bam.Shell
{
    [Serializable]
    public class LifeCycleProvider : CommandLineTestInterface
    {   
        [ArgZero("config", "Write the default config file backing up the current file if it exists")]
        public void Config()
        {
            BamSettings settings = BamSettings.Load();
            if(!settings.IsValid(msgs => OutLine(msgs, ConsoleColor.Magenta)))
            {
                settings.Save(bak => OutLineFormat("Backed up existing file: {0}", ConsoleColor.DarkYellow, bak));
            }            
        }

        [ArgZero("init", "Add BamFramework to the current csproj")]
        public void Initialize()
        {
            Init.AspNetRazorInit();
        }


        [ArgZero("clean", "Clear all dynamic types and namespaces from the dynamic type manager")]
        public void CleanGeneratedTypes()
        {
            OutLine("Deleting ALL dynamic types from the local DynamicTypeManager", ConsoleColor.Yellow);
            DynamicTypeManager mgr = new DynamicTypeManager();
            mgr.DynamicTypeDataRepository.Query<DynamicTypePropertyDescriptor>(p => p.Id > 0).Each(p => mgr.DynamicTypeDataRepository.Delete(p));
            mgr.DynamicTypeDataRepository.Query<DynamicTypeDescriptor>(d => d.Id > 0).Each(d => mgr.DynamicTypeDataRepository.Delete(d));
            mgr.DynamicTypeDataRepository.Query<DynamicNamespaceDescriptor>(d => d.Id > 0).Each(d => mgr.DynamicTypeDataRepository.Delete(d));
            OutLine("Done", ConsoleColor.DarkYellow);
        }

        [ArgZero("build", "Write a docker file and build a docker image.")]
        public void Build()
        {
            DirectoryInfo projectParent = ShellProvider.GetProjectParentDirectoryOrExit(out FileInfo csprojFile);
            BamSettings settings = ShellProvider.GetSettings();
            HandlebarsDirectory handlebars = ShellProvider.GetHandlebarsDirectory();
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

        [ArgZero("push", "Tag the docker image and push it to the bamapps docker registry.")]
        public void Push()
        {
            DirectoryInfo projectParent = ShellProvider.GetProjectParentDirectoryOrExit(out FileInfo csprojFile);
            string projectName = Path.GetFileNameWithoutExtension(csprojFile.Name);
            BamSettings settings = ShellProvider.GetSettings();
            string startDir = Environment.CurrentDirectory;
            settings.DockerPath.ToStartInfo($"tag {projectName} bamapps/images:{projectName}").Run(msg => OutLine(msg, ConsoleColor.Cyan));
            settings.DockerPath.ToStartInfo($"push bamapps/images:{projectName}").Run(msg=> OutLine(msg, ConsoleColor.DarkCyan));
            Environment.CurrentDirectory = startDir;
        }

        public void Run(string name, Action<string> output = null, Action<string> error = null)
        {
            string assemblyPath = GetArgument("assembly",
                "Please specify the path to the assembly containing cli switches to execute");
            string className = GetArgument("class",
                "Please enter the name of the class containing the cli switch to execute");
            if (!File.Exists(assemblyPath))
            {
                OutLineFormat("Specified assembly does not exist: {0}", ConsoleColor.Magenta, assemblyPath);
                Exit(1);
            }

            FileInfo file = new FileInfo(assemblyPath);
            Assembly targetAssembly = Assembly.LoadFile(file.FullName);
            Type targetType = targetAssembly.GetType(className);
            if (targetType == null)
            {
                targetType = targetAssembly.GetTypes()
                    .FirstOrDefault(t => $"{t.Namespace}.{t.Name}".Equals(className) || className.Equals(t.Name));
                if (targetType == null)
                {
                    OutLineFormat("Specified class was not found in the specified assembly: className = {0}", ConsoleColor.Magenta, className);
                    Exit(1);
                }
            }

            ConsoleLogger logger = new ConsoleLogger {AddDetails = false};
            ExecuteSwitches(Arguments, targetType, false, logger);
        }
    }
}
