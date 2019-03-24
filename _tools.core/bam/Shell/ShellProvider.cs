using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Bam.Net;
using Bam.Net.Presentation.Handlebars;
using Bam.Net.Testing;
using Bam.Shell.Jobs;

namespace Bam.Shell
{
    public abstract class ShellProvider : CommandLineTestInterface
    {
        public abstract void List(Action<string> output = null, Action<string> error = null);
        public abstract void Add(Action<string> output = null, Action<string> error = null);
        public abstract void Show(Action<string> output = null, Action<string> error = null);
        public abstract void Set(Action<string> output = null, Action<string> error = null);
        public abstract void Remove(Action<string> output = null, Action<string> error = null);
        public abstract void Run(Action<string> output = null, Action<string> error = null);

        public virtual void Edit(Action<string> output = null, Action<string> error = null)
        {
            OutLineFormat("Edit is not implemented for the current shell provider: {0}", GetType().FullName);
        }
        
        static HandlebarsDirectory _handlebarsDirectory;
        static object _handlebarsLock = new object();
        public static HandlebarsDirectory GetHandlebarsDirectory()
        {
            return _handlebarsLock.DoubleCheckLock(ref _handlebarsDirectory, () =>
            {
                DirectoryInfo bamDir = Assembly.GetExecutingAssembly().GetFileInfo().Directory;
                return new HandlebarsDirectory(Path.Combine(bamDir.FullName, "Templates"));
            });
        }

        string _providerType;
        public string ProviderType
        {
            get
            {
                if (string.IsNullOrEmpty(_providerType))
                {
                    Type type = this.GetType();
                    if (type.Name.EndsWith("Provider"))
                    {
                        _providerType = type.Name.Truncate("Provider".Length);
                    }
                    else
                    {
                        _providerType = type.Name;
                    }
                }

                return _providerType;
            }
        }
        
        public virtual void RegisterArguments()
        {
            AddValidArgument("name", $"The name of the {ProviderType} to work with");
            AddValidArgument("format", "The desired output format: json | yaml");
        }
        
        protected virtual ProviderArguments GetProviderArguments()
        {
            string targetName = Arguments.Contains("name")
                ? Arguments["name"]
                : (
                    Arguments.Contains($"{ProviderType.ToLowerInvariant()}Name")
                        ? Arguments[$"{ProviderType.ToLowerInvariant()}Name"]
                        : (
                            GetTypeNameArgument(ProviderType.ToLowerInvariant(), $"Please enter the name of the {ProviderType.ToLowerInvariant()}") 
                            ??
                            GetArgument($"Please enter the name of the {ProviderType.ToLowerInvariant()}")
                          )
                  );
            
            return new ProviderArguments()
            {
                ProviderType = ProviderType,
                ProviderContextTarget = targetName,
            };
        }

        public string Serialize(object data)
        {
            return data.Serialize(GetFormat());
        }
        
        protected SerializationFormat GetFormat()
        {
            if (Arguments.Contains("format"))
            {
                SerializationFormat format = Arguments["format"].ToEnum<SerializationFormat>();
                if (format != SerializationFormat.Json && format != SerializationFormat.Yaml)
                {
                    format = SerializationFormat.Yaml;
                }

                return format;
            }

            return SerializationFormat.Yaml;
        }
        
        
        protected string GetTypeNameArgument(string type, string prompt = null)
        {
            if (Arguments.Contains(type))
            {
                return Arguments[type];
            }

            if (Arguments.Contains($"{type}Name"))
            {
                return Arguments[$"{type}Name"];
            }

            return Prompt(prompt ?? $"Please enter the {type} name");
        } 
        
        public static DirectoryInfo FindProjectParent(out FileInfo csprojFile)
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
        
        public static BamSettings GetSettings()
        {
            BamSettings settings = BamSettings.Load();
            if (!settings.IsValid(msg => OutLine(msg, ConsoleColor.Red)))
            {
                Exit(1);
            }

            return settings;
        }

        public static DirectoryInfo GetProjectParentDirectoryOrExit()
        {
            return GetProjectParentDirectoryOrExit(out FileInfo ignore);
        }

        public static DirectoryInfo GetProjectParentDirectoryOrExit(out FileInfo csprojFile)
        {
            DirectoryInfo projectParent = FindProjectParent(out csprojFile);
            if (csprojFile == null)
            {
                OutLine("Can't find csproj file", ConsoleColor.Magenta);
                Exit(1);
            }

            return projectParent;
        }
        
        protected FileInfo FindProjectFile()
        {
            FindProjectParent(out FileInfo csprojFile);
            return csprojFile;
        }
        
        protected void EnsureDirectoryExists(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
        }
    }
}