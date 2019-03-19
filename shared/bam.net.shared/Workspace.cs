using System;
using System.Collections.Generic;
using System.IO;
using Bam.Net.Presentation.Handlebars;

namespace Bam.Net
{
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

        static Workspace _current;
        static object _currentLock = new object();
        public static Workspace Current
        {
            get { return _currentLock.DoubleCheckLock(ref _current, () => ForApplication()); }
        }

        public static Workspace ForClass<T>(IApplicationNameProvider applicationNameProvider = null)
        {
            return ForClass(typeof(T), applicationNameProvider);
        }
        
        public static Workspace ForClass(Type type, IApplicationNameProvider applicationNameProvider = null)
        {
            applicationNameProvider = applicationNameProvider ?? ProcessApplicationNameProvider.Current;
            Workspace applicationWorkspace = ForApplication(applicationNameProvider);
            string directoryPath =
                System.IO.Path.Combine(applicationWorkspace.Root.FullName, ProcessMode.Current.Mode.ToString(), $"{type.Namespace}.{type.Name}");
            return new Workspace()
                {ApplicationNameProvider = applicationNameProvider, Root = new DirectoryInfo(directoryPath)};
        }
        
        public static Workspace ForApplication(IApplicationNameProvider applicationNameProvider = null)
        {
            applicationNameProvider = applicationNameProvider ?? ProcessApplicationNameProvider.Current;
            string directoryPath = System.IO.Path.Combine(BamPaths.BamHome, "apps", applicationNameProvider.GetApplicationName());
            return new Workspace()
                {ApplicationNameProvider = applicationNameProvider, Root = new DirectoryInfo(directoryPath)};
        } 
    }
}