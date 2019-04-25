using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Bam.Net;
using Bam.Net.Application;
using Bam.Net.CommandLine;
using Bam.Net.Presentation.Handlebars;


namespace Bam.Shell.Pack
{
    public class DockerPackageProvider : Bam.Shell.Pack.PackageProvider
    {
        
        public override void Build(Action<string> output = null, Action<string> error = null)
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
        
        public override void Pack(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException("Pack is not implemented");
        }   
        
        /// <summary>
        /// Tag the docker image and push it to the bamapps docker registry.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="error"></param>
        public override void Push(Action<string> output = null, Action<string> error = null)
        {
            DirectoryInfo projectParent = ShellProvider.GetProjectParentDirectoryOrExit(out FileInfo csprojFile);
            string projectName = Path.GetFileNameWithoutExtension(csprojFile.Name);
            BamSettings settings = ShellProvider.GetSettings();
            string startDir = Environment.CurrentDirectory;
            settings.DockerPath.ToStartInfo($"tag {projectName} bamapps/images:{projectName}").Run(msg => OutLine(msg, ConsoleColor.Cyan));
            settings.DockerPath.ToStartInfo($"push bamapps/images:{projectName}").Run(msg=> OutLine(msg, ConsoleColor.DarkCyan));
            Environment.CurrentDirectory = startDir;
        }        
        
        public override void Pull(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException("Pull is not implemented");
        }        
    }
}