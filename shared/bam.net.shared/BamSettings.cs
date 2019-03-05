using Bam.Net.Application;
using Bam.Net.CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Bam.Net
{
    public class BamSettings
    {
        public BamSettings()
        {
            ArgumentPrefix = ParsedArguments.DefaultArgPrefix;
            Environment = StandardEnvironments.Development;
        }

        public string ArgumentPrefix { get; set; }

        public StandardEnvironments Environment { get; set; }
        public string GitPath { get; set; }
        public string DotNetPath { get; set; }
        public string NpxPath { get; set; }
        public string NodePath { get; set; }
        public string NpmPath { get; set; }
        public string DockerPath { get; set; }

        public bool IsValid(Action<string> notValid)
        {
            if (string.IsNullOrEmpty(GitPath))
            {
                GitPath = GetPath("git");
            }
            if (string.IsNullOrEmpty(DotNetPath))
            {
                DotNetPath = GetPath("dotnet");
            }
            if (string.IsNullOrEmpty(NpxPath))
            {
                NpxPath = GetPath("npx");
            }
            if (string.IsNullOrEmpty(NodePath))
            {
                NodePath = GetPath("node");
            }
            if (string.IsNullOrEmpty(NpmPath))
            {
                NpmPath = GetPath("npm");
            }
            if (string.IsNullOrEmpty(DockerPath))
            {
                DockerPath = GetPath("docker");
            }

            StringBuilder message = new StringBuilder();
            if (string.IsNullOrEmpty(GitPath.Trim()))
            {
                message.Append("git was not found, please specify the path to the git executable");
            }
            if (string.IsNullOrEmpty(DotNetPath.Trim()))
            {
                message.Append("dotnet was not found, please specify the path to the dotnet executable");
            }
            if (string.IsNullOrEmpty(NpxPath.Trim()))
            {
                message.AppendLine("npx was not found, please specify the path to the npx executable");
            }
            if (string.IsNullOrEmpty(NodePath.Trim()))
            {
                message.AppendLine("node was not found, please specify the path to the node executable");
            }
            if (string.IsNullOrEmpty(DockerPath.Trim()))
            {
                message.AppendLine("docker was not found, please specify the path to the docker executable");
            }
            if (!string.IsNullOrEmpty(message.ToString()))
            {
                notValid(message.ToString());
                return false;
            }
            return true;
        }

        public static BamSettings Load(bool createIfMissing)
        {
            return Load(null, createIfMissing);
        }

        public static BamSettings Load(string path = null, bool createIfMissing = false)
        {
            path = path ?? Path.Combine(".", $"bam-{OSInfo.Current.ToString()}.yaml");
            if (!File.Exists(path) && createIfMissing)
            {
                BamSettings settings = new BamSettings
                {
                    GitPath = GetPath("git"),
                    DotNetPath = GetPath("dotnet"),
                    NpxPath = GetPath("npx"),
                    NodePath = GetPath("node"),
                    NpmPath = GetPath("npm"),
                    DockerPath = GetPath("docker")
                };
                settings.ToYamlFile(path);
            }
            return path.FromYamlFile<BamSettings>();
        }
        
        public string Save(Action<string> moved = null)
        {
            return Save(null, moved);
        }

        public string Save(string path = null, Action<string> moved = null)
        {
            path = path ?? Path.Combine(".", $"bam-{OSInfo.Current.ToString()}.yaml");
            if (File.Exists(path))
            {
                string backUp = path.GetNextFileName();
                File.Move(path, backUp);
                moved?.Invoke(backUp);
            }
            this.ToYamlFile(path);
            return path;
        }

        public static string GetPath(string fileName)
        {
            return OSInfo.GetPath(fileName);
        }
    }
}
