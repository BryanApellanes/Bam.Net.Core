using System;
using System.Collections.Generic;
using System.IO;
using Bam.Net.Configuration;
using Bam.Net.Logging;
using Bam.Net.Presentation.Handlebars;

namespace Bam.Net
{
    /// <summary>
    /// Provides an interface to a specific place in the filesystem for an application.
    /// </summary>
    public class Workspace
    {
        public IApplicationNameProvider ApplicationNameProvider { get; set; }
        public DirectoryInfo Root { get; set; }
        
        public DirectoryInfo CreateDirectory(params string[] pathSegments)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Path(pathSegments));
            if (!directoryInfo.Exists)
            {
                return System.IO.Directory.CreateDirectory(directoryInfo.FullName);
            }

            return directoryInfo;
        }
        
        public FileInfo CreateFile(params string[] pathSegments)
        {
            FileInfo file = new FileInfo(Path(pathSegments));
            if (!file.Exists)
            {
                System.IO.File.Create(file.FullName).Dispose();
            }

            return file;
        }

        /// <summary>
        /// Get a directory for the specified path relative to the workspace
        /// </summary>
        /// <param name="pathSegments"></param>
        /// <returns></returns>
        public DirectoryInfo Directory(params string[] pathSegments)
        {
            return new DirectoryInfo(Path(pathSegments));
        }
        
        public FileInfo File(params string[] pathSegments)
        {
            return new FileInfo(Path(pathSegments));
        }
        
        public string Path(params string[] pathSegments)
        {
            List<string> fileSegments = new List<string>();
            fileSegments.Add(Root.FullName);
            fileSegments.AddRange(pathSegments);
            return System.IO.Path.Combine(fileSegments.ToArray());
        }

        /// <summary>
        /// Output to the console and write that output to the Workspace console log.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
            string message = $"{string.Format(format, args)}\r\n";
            FileInfo file = new FileInfo(Path("Console"));
            if (file.Exists && file.Length >= 1048576)
            {
                file = file.GetNextFile();
            }
            message.SafeAppendToFile(file.FullName);
        }
        
        /// <summary>
        /// Save the specified object instance as a yaml file
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public FileInfo Save(object instance)
        {
            Args.ThrowIfNull(instance, "instance");
            Type type = instance.GetType();
            FileInfo file = CreateFile($"{type.Namespace}", $"{type.Name}.yaml");
            instance.ToYamlFile(file);
            return file;
        }

        public T Load<T>()
        {
            Type type = typeof(T);
            FileInfo file = CreateFile($"{type.Namespace}", $"{type.Name}.yaml");
            return file.FromYamlFile<T>();
        }

        TextFileLogger _logger;
        public ILogger CreateLogger<T>() where T : TextFileLogger, new()
        {
            if (_logger == null)
            {
                _logger = new T();
                _logger.Folder = Root;
            }

            return _logger;
        }
        
        static Workspace _current;
        static object _currentLock = new object();
        public static Workspace Current
        {
            get { return _currentLock.DoubleCheckLock(ref _current, () => ForApplication()); }
        }
        
        public static Workspace ForType<T>(IApplicationNameProvider applicationNameProvider = null)
        {
            return ForType(typeof(T), applicationNameProvider);
        }
        
        public static Workspace ForType(Type type, IApplicationNameProvider applicationNameProvider = null)
        {
            applicationNameProvider = applicationNameProvider ?? ProcessApplicationNameProvider.Current;
            Workspace applicationWorkspace = ForApplication(applicationNameProvider);
            string directoryPath =
                System.IO.Path.Combine(applicationWorkspace.Root.FullName, ProcessMode.Current.Mode.ToString(), $"{type.Namespace}.{type.Name}");
            return new Workspace()
                {ApplicationNameProvider = applicationNameProvider, Root = new DirectoryInfo(directoryPath)};
        }

        public static Workspace ForProcess()
        {
            return ForApplication(ProcessApplicationNameProvider.Current);
        }
        
        public static Workspace ForApplication(IApplicationNameProvider applicationNameProvider = null)
        {
            applicationNameProvider = applicationNameProvider ?? ProcessApplicationNameProvider.Current;
            Log.Trace(typeof(Workspace), "Workspace using applicationNameProvider of type ({0})", applicationNameProvider?.GetType().Name);
            string directoryPath = System.IO.Path.Combine(BamHome.Apps, applicationNameProvider.GetApplicationName());
            return new Workspace()
                {ApplicationNameProvider = applicationNameProvider, Root = new DirectoryInfo(directoryPath)};
        }

        public static Workspace ForApplication(string applicationName)
        {
            return new Workspace()
            {
                ApplicationNameProvider = new StaticApplicationNameProvider(applicationName),
                Root = new DirectoryInfo(System.IO.Path.Combine(BamHome.Apps, applicationName))
            };
        }
    }
}