/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Bam.Net;
using Bam.Net.Configuration;
using Bam.Net.Presentation.Html;
using Bam.Net.Data;
using Bam.Net.ServiceProxy;

using System.Web;
using Bam.Net.Caching.File;
using NLog.Targets;

namespace Bam.Net.Server
{
    [Proxy("fs")]
    [RoleRequired("Admin")]
    public partial class Fs
    {
        FileCache _cache;
		public Fs()
		{
            _cache = new BinaryFileCache();
		}
        
        public Fs(string specifiedPath): this()
        {
			RootDir = new DirectoryInfo(specifiedPath);
			SpecifiedPath = specifiedPath;
        }

        public Fs(DirectoryInfo rootDir):this()
        {
            RootDir = rootDir;
        }

        [Exclude]
        public static void RegisterProxy(string appName)
        {
            ServiceProxySystem.Register<Fs>(new Fs(appName));
        }

        [Exclude]
        public static void UnregisterProxy()
        {
            ServiceProxySystem.Unregister<Fs>();
        }
   
        [Exclude]
        public DirectoryInfo RootDir
        {
            get;
            set;
        }

        [Exclude]
        public string Root
        {
            get => CleanAppPath(RootDir.FullName) + Path.DirectorySeparatorChar;
            set => this.RootDir = new DirectoryInfo(value);
        }

        string _currentDirectory;
        [Exclude]
        public string CurrentDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_currentDirectory))
                {
                    _currentDirectory = Root;
                }

                return _currentDirectory;
            }
            set => _currentDirectory = value;
        }

        public string SpecifiedPath { get; set; }
        
        public event FsEvent DirectoryCreated;
        private void OnDirectoryCreated(string path)
        {
            if (DirectoryCreated != null)
            {
                DirectoryCreated(path);
            }
        }

        public event FsEvent FileWritten;
        private void OnFileWritten(string path)
        {
            FileWritten?.Invoke(path);
        }

        public event FsEvent FileAppendedTo;
        private void OnFileAppendedTo(string path)
        {
            FileAppendedTo?.Invoke(path);
        }

        public void CreateDirectory(string directory)
        {
            string path = GetAbsolutePath(directory);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                OnDirectoryCreated(path);
            }
        }

        /// <summary>
        /// Write the specified content to the file at the specified path.  If overwrite is false
        /// and the file exists the attempt to write fails with an InvalidOperationException.
        /// </summary>
        /// <param name="relativeFilePath"></param>
        /// <param name="contentToWrite"></param>
        /// <param name="overwrite"></param>
        public void Write(string relativeFilePath, string contentToWrite, bool overwrite = true)
        {
            string path = EnsurePath(relativeFilePath);

            path.SafeWriteFile(contentToWrite, overwrite);

            OnFileWritten(path);
        }

        private string EnsurePath(string relativeFilePath)
        {
            string path = GetAbsolutePath(EnsureRelative(relativeFilePath));
            FileInfo file = new FileInfo(path);
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            return path;
        }

        public void Write(string relativeFilePath, byte[] contentToWrite)
        {
            string path = EnsurePath(relativeFilePath);
            File.WriteAllBytes(path, contentToWrite);
        }

        public byte[] ReadBytes(string relativeFilePath)
        {
            string path = GetAbsolutePath(EnsureRelative(relativeFilePath));
            return _cache.GetBytes(new FileInfo(path));
        }
		public string ReadAllText(params string[] pathSegments)
		{
			return ReadAllText(Path.Combine(pathSegments));
		}
        public string ReadAllText(string relativeFilePath)
        {
            string path = GetAbsolutePath(EnsureRelative(relativeFilePath));
            return _cache.GetText(new FileInfo(path));
        }

        public void Append(string relativeFilePath, string text)
        {
            string path = GetAbsolutePath(EnsureRelative(relativeFilePath));
            text.SafeAppendToFile(path);

            OnFileAppendedTo(path);
        }

        public bool FileExists(out string absolutePath, params string[] pathSegmentsToCombine)
        {
            return FileExists(Path.Combine(pathSegmentsToCombine), out absolutePath);
        }
		public bool FileExists(params string[] pathSegmentsToCombine)
		{
			return FileExists(Path.Combine(pathSegmentsToCombine));
		}

		public bool FileExists(string relativePath)
		{
            return FileExists(relativePath, out string ignore);
        }

        public bool FileExists(string relativePath, out string absolutePath)
        {
            relativePath = EnsureRelative(relativePath);
			absolutePath = GetAbsolutePath(relativePath);
            return File.Exists(absolutePath);
        }

        /// <summary>
        /// Move the file from src to dest.  The source
        /// and dest must be relative to the current Fs
        /// root.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        public void MoveFile(string src, string dest)
        {
            string s = GetAbsolutePath(EnsureRelative(src));
            string d = GetAbsolutePath(EnsureRelative(dest));
            File.Move(s, d);
        }

        public void MoveDirectory(string src, string dest)
        {
            string s = GetAbsolutePath(EnsureRelative(src));
            string d = GetAbsolutePath(EnsureRelative(dest));
            Directory.Move(s, d);
        }

        private static string EnsureRelative(string relativePath)
        {
            if (!relativePath.StartsWith("~"))
            {
                relativePath = $"~{relativePath}";
            }
            return relativePath;
        }

        public bool DirectoryExists(string relativePath)
        {
            relativePath = EnsureRelative(relativePath);

            return Directory.Exists(GetAbsolutePath(relativePath));
        }

        public string[] GetDirectories(string relativePath)
        {
            relativePath = EnsureRelative(relativePath);

            DirectoryInfo dir = new DirectoryInfo(GetAbsolutePath(relativePath));
            return dir.GetDirectories().Select(d => d.Name).ToArray();
        }
		public DirectoryInfo GetDirectory(string relativePath)
		{
			relativePath = EnsureRelative(relativePath);

			return new DirectoryInfo(GetAbsolutePath(relativePath));
		}

		public FileInfo[] GetFiles(string relativePath, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			relativePath = EnsureRelative(relativePath);

			DirectoryInfo dir = new DirectoryInfo(GetAbsolutePath(relativePath));
			if (dir.Exists)
			{
				return dir.GetFiles(searchPattern, searchOption);
			}
			else
			{
				return new FileInfo[] { };
			}
		}

		public FileInfo GetFile(string relativePath)
		{
			relativePath = EnsureRelative(relativePath);

			return new FileInfo(GetAbsolutePath(relativePath));
		}

		public void WriteFile(string relativeFilePath, string contentToWrite, bool overwrite = true)
		{
			string path = GetAbsolutePath(EnsureRelative(relativeFilePath));
			FileInfo file = new FileInfo(path);
			if (!file.Directory.Exists)
			{
				file.Directory.Create();
			}

			path.SafeWriteFile(contentToWrite, overwrite);

			OnFileWritten(path);
		}

		[Exclude]
		public string GetAbsolutePath(params string[] pathSegments)
		{
			string path = Path.Combine(pathSegments);
			return GetAbsolutePath(path);
		}

        [Exclude]
        public string GetAbsolutePath(string relativePath)
        {
            string result = "";
            if (relativePath.StartsWith("~"))
            {
                relativePath = relativePath.Substring(1, relativePath.Length - 1);
                string path = CleanAppPath($"{Root}{relativePath}");
                result = path;
            }
            else
            {
                result = relativePath;
            }

            return CleanUserPath(result);
        }

        /// <summary>
        /// Normalizes the specified path and resolves ~ anywhere in the path to the
        /// users home directory.
        public string CleanUserPath(string path)
        {
            return CleanPath(path, ResolveUserHome);
        }

        /// <summary>
        /// Normalizes the specified path and resolves ~ anywhere in the path to the
        /// apps home directory.
        public string CleanAppPath(string path)
        {
            return CleanPath(path, ResolveAppHome);
        }
        
        /// <summary>
        /// Normalizes the specified path and resolves ~ anywhere in the path to the
        /// result of the specified tildeResolver.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
		public static string CleanPath(string path, Func<string, string> tildeResolver)
		{
            string[] pathSegments = path.Split('\\', '/');
            if (OSInfo.IsUnix)
            {
                if (path.StartsWith("/"))
                {
                    return tildeResolver($"/{Path.Combine(pathSegments)}");
                }

                return tildeResolver(Path.Combine(pathSegments));
            }
            else
            {
                if (pathSegments[0].EndsWith(":")) // C:
                {
                    pathSegments[0] = pathSegments[0] + Path.DirectorySeparatorChar;
                }

                return tildeResolver(Path.Combine(pathSegments));
            }			
		}

        public string ResolveAppHome(string path)
        {
            if (path.Contains("~"))
            {
                string firstPart = path.ReadUntil('~', out string secondPart);

                return Path.Combine(RootDir.FullName, secondPart.RemainderAfter('/'));
            }

            return path;
        }
        
        public static string ResolveUserHome(string path)
        {
            if (path.Contains("~"))
            {
                string firstPart = path.ReadUntil('~', out string secondPart);
                
                return Path.Combine(BamHome.UserHome, secondPart.RemainderAfter('/'));
            }

            return path;
        }
    }

}