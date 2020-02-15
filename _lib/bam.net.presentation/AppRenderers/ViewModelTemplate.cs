using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Bam.Net.Caching.File;
using Bam.Net.Server;

namespace Bam.Net.Presentation.AppRenderers
{
    public class ViewModelTemplate
    {
        private static Dictionary<string, TextFileCache> _fileCaches;
        private static Dictionary<string, Type> _actionProviderTypes;
        private static Dictionary<string, object> _actionProviders;
        private static Dictionary<string, ViewModel> _viewModels;

        public ViewModelTemplate()
        {    if (_actionProviders == null)
            {
                _actionProviders = new Dictionary<string, object>();
            }

            if (_actionProviders == null)
            {
                _actionProviders = new Dictionary<string, object>();
            }

            if (_actionProviderTypes == null)
            {
                _actionProviderTypes = new Dictionary<string, Type>();
            }
        }
        
        public string FileSystemPath { get; set; }
        /// <summary>
        /// Namespace qualified name of the action provider.  Or AssemblyQualified name to be more
        /// restrictive.
        /// </summary>
        public string ActionProvider { get; set; }

        public string ViewModelId => $"{FileSystemPath}::{ActionProvider}";

        public string GetShortName()
        {
            return Path.GetFileNameWithoutExtension(FileSystemPath);
        }

        public string GetLongName()
        {
            string fileExtension = Path.GetExtension(FileSystemPath);
            return FileSystemPath.Truncate(fileExtension.Length);
        }
        
        public T GetActionProvider<T>(AppConf appConf)
        {
            return (T) GetActionProvider(appConf);
        }

        public object GetActionProvider(AppConf appConf)
        {
            string actionProviderName = ParseActionProviderName(appConf);
            if (string.IsNullOrEmpty(actionProviderName))
            {
                Logging.Log.Warn("Unable to parse action provider for '{0}': \r\n\t html: \r\n{1}", FileSystemPath, Exec.Try(() => File.ReadAllText(FileSystemPath)));
                return null;
            }
            
            if (!_actionProviderTypes.ContainsKey(actionProviderName))
            {
                Type actionProviderType = ResolveActionProviderType(appConf);
                _actionProviderTypes.Add(actionProviderName, actionProviderType);
            }

            return _actionProviderTypes[actionProviderName].Construct();
        }

        public string GetSource()
        {
            Args.ThrowIfNullOrEmpty(FileSystemPath, "Path");
            
            if (_fileCaches == null)
            {
                _fileCaches = new Dictionary<string, TextFileCache>();
            }

            string fileExtension = System.IO.Path.GetExtension(FileSystemPath).ToLowerInvariant();
            if (!_fileCaches.ContainsKey(fileExtension))
            {
                _fileCaches.Add(fileExtension, new TextFileCache(fileExtension));
            }

            return _fileCaches[fileExtension].GetText(new FileInfo(FileSystemPath));
        }
        
        public ViewModel<T> GetViewModel<T>(AppConf appConf)
        {
            string name = System.IO.Path.GetFileNameWithoutExtension(FileSystemPath);
            return new ViewModel<T>() {Name = name, ActionProvider = GetActionProvider<T>(appConf)};
        }

        public ViewModel GetViewModel(AppConf appConf)
        {
            string name = System.IO.Path.GetFileNameWithoutExtension(FileSystemPath);
            ViewModel result = new ViewModel
            {
                Name = name, 
                ActionProvider = GetActionProvider(appConf) ?? new object(), 
                ViewModelId = ViewModelId
            };
            return result;
        }

        public virtual ViewModel Load(AppConf appConf)
        {
            if (_viewModels == null)
            {
                _viewModels = new Dictionary<string, ViewModel>();
            }
            if (string.IsNullOrEmpty(ActionProvider))
            {
                ParseActionProviderName(appConf);
            }
            if (!_viewModels.ContainsKey(ViewModelId))
            {
                _viewModels.Add(ViewModelId, GetViewModel(appConf));
            }

            return _viewModels[ViewModelId];
        }
        
        /// <summary>
        /// Reads the provider name from the template file
        /// </summary>
        /// <param name="appConf"></param>
        /// <returns></returns>
        public virtual string ParseActionProviderName(AppConf appConf)
        {
            ActionProvider = HtmlElement.DataAttribute("actionprovider");
            return ActionProvider;
        }

        private XDocument _document;
        protected virtual XDocument Document => _document ?? (_document = XDocument.Load(FileSystemPath));

        protected virtual XElement HtmlElement => Document.Element("html");

        protected virtual Type ResolveActionProviderType(AppConf appConf)
        {
            if (string.IsNullOrEmpty(FileSystemPath))
            {
                Args.Throw<InvalidOperationException>("ViewModel path not set");
            }
            
            if (string.IsNullOrEmpty(ActionProvider))
            {
                ActionProvider = ParseActionProviderName(appConf);
            }

            if (string.IsNullOrEmpty(ActionProvider))
            {
                Args.Throw<InvalidOperationException>("Unable to resolve action provider name from viewmodel: {0}", FileSystemPath);
            }

            DirectoryInfo binDir = new DirectoryInfo(System.IO.Path.Combine(appConf.AppRoot.Root, appConf.BinDir));
            foreach (FileInfo fileInfo in binDir.GetFiles("*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    Assembly assembly = Assembly.LoadFile(fileInfo.FullName);
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (ActionProvider.Equals(type.FullName) ||
                            ActionProvider.Equals(type.AssemblyQualifiedName))
                        {
                            return type;
                        }
                    }
                }
                catch (Exception ex)
                {
                    appConf?.Logger.Warning("Error resolving action provider type for viewmodel {0}: {1}", FileSystemPath, ex.Message);
                }
            }

            return null;
        }
    }
}