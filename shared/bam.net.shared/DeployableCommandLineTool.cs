using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using Bam.Net.CommandLine;
using Bam.Net.Testing;
using Bam.Net.Web;

namespace Bam.Net
{
    public class DeployableCommandLineTool : CommandLineTool
    {
        public DeployableCommandLineTool()
        {
            AddValidArgument("toolName", "Install: The name of the tool to install");
        }

        [ConsoleAction("RemoteInstall", "Install a tool on a specified remote host.")]
        public void Deploy()
        {
            string toolName = GetToolName();
            OutLine("This is not fully implemented", ConsoleColor.Yellow);
            OutLine("Ssh to the remote then download and run http://bamapps.net/download?fileName=install.sh, use 'source install.sh' to set path after install.", ConsoleColor.Cyan);
        }
        
        [ConsoleAction("Install", "Download and install the latest version of a tool.")]
        public void Install()
        {
            string toolName = GetToolName();
            string homeDir = BamHome.UserHome;
            string runtime = OSInfo.BuildRuntimeName;
            string zipFileName = $"bamtoolkit-{toolName}-{runtime}.zip";

            string tmpDir = Path.Combine(homeDir, ".bam", "tmp");
            string binDir = Path.Combine(homeDir, ".bam", "toolkit", runtime, toolName);
            string downloadPath = Path.Combine(tmpDir, zipFileName);

            if (Directory.Exists(binDir))
            {
                Directory.Delete(binDir, true);
            }

            if (!Directory.Exists(tmpDir))
            {
                Directory.CreateDirectory(tmpDir);
            }

            OutLineFormat("downloading {0}", ConsoleColor.Cyan, toolName);
            Http.Get($"http://bamapps.net/download?fileName={zipFileName}", downloadPath);
            OutLineFormat("file downloaded to {0}", ConsoleColor.Green, downloadPath);
            OutLineFormat("unzipping {0} to {1}", downloadPath, binDir);
            ZipFile.ExtractToDirectory(downloadPath, binDir);
            OutLineFormat("unzipping complete", ConsoleColor.Green);
            OutLineFormat("deleting file {0}", ConsoleColor.Cyan, downloadPath);
            File.Delete(downloadPath);
            OutLineFormat("delete complete", ConsoleColor.Green);
            OutLineFormat($"add {binDir} to your path", ConsoleColor.Yellow);
        }

        string toolName = null;
        protected virtual string GetToolName()
        {
            return toolName ??= GetArgument("toolName", "Enter the name of the tool to install");
        }

        protected virtual string GetCurrentToolName()
        {
            Process process = Process.GetCurrentProcess();
            FileInfo main = new FileInfo(process.MainModule.FileName);
            return Path.GetFileNameWithoutExtension(main.Name);
        }

        protected virtual string GetZipFileName()
        {
            string toolName = GetToolName();
            string homeDir = BamHome.UserHome;
            string runtime = OSInfo.BuildRuntimeName;
            return $"bamtoolkit-{toolName}-{runtime}.zip";
        }
    }
}