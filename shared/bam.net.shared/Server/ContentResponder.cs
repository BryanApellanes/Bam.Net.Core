/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net;
using Bam.Net.Caching.File;
using Bam.Net.Javascript;
using Bam.Net.Logging;
using Bam.Net.Server.Renderers;
using Bam.Net.ServiceProxy;
using Bam.Net.ServiceProxy.Secure;
using Bam.Net.UserAccounts.Data;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Bam.Net.CommandLine;
using Bam.Net.Server.Meta;
using Bam.Net.Data.Repositories;
using Bam.Net.Configuration;
using Bam.Net.Presentation;
using Bam.Net.Services;

namespace Bam.Net.Server
{
    /// <summary>
    /// The primary responder for all content files found in ~s:/ (defined as BamServer.ContentRoot)
    /// </summary>
    public partial class ContentResponder : Responder, IInitialize<ContentResponder>
    {
        public const string CommonFolder = "common";
        
        static string contentRootConfigKey = "ContentRoot";
        static string defaultRoot = BamHome.Content;
        public const string IncludeFileName = "include.js";
        public const string LayoutFileExtension = ".layout";
        public const string HostAppMapFile = "hostAppMaps.json";

        public ContentResponder(BamConf conf, ILogger logger, ITemplateManager commonTemplateManager = null)
            : base(conf, logger)
        {
            Fs commonRoot = new Fs(new DirectoryInfo(Path.Combine(ServerRoot.Root, CommonFolder)));
            
            ContentRoot = conf?.ContentRoot ?? DefaultConfiguration.GetAppSetting(contentRootConfigKey, defaultRoot);
            ServerRoot = new Fs(ContentRoot);
            TemplateDirectoryNames = new List<string>(new string[] { "pages", "views", "templates" });
            CommonTemplateManager = commonTemplateManager;
            FileCachesByExtension = new Dictionary<string, FileCache>();
            HostAppMappings = new Dictionary<string, HostAppMap>();     
            CommonContentLocator = ContentLocator.Load(commonRoot);
            InitializeFileExtensions();
            InitializeCaches();
        }

        public ContentResponder(ILogger logger, ITemplateManager templateManager = null) : this(BamConf.Load(), logger, templateManager)
        { }

        public string ContentRoot { get; set; }

        public ContentLocator CommonContentLocator
        {
            get;
            private set;
        }
        
        public AppMetaInitializer AppMetaInitializer { get; set; }
        public List<string> TemplateDirectoryNames { get; set; }
        public List<string> FileExtensions { get; set; }
        public List<string> TextFileExtensions { get; set; }
        public Dictionary<string, HostAppMap> HostAppMappings { get; set; }
        protected Dictionary<string, FileCache> FileCachesByExtension { get; set; }
        protected void InitializeFileExtensions()
        {
            FileExtensions = new List<string> { ".html", ".htm", ".js", ".json", ".css", ".yml", ".yaml", ".txt", ".md", ".layout", ".png", ".jpg", ".jpeg", ".gif", ".woff" };
            TextFileExtensions = new List<string> { ".html", ".htm", ".js", ".json", ".css", ".yml", ".yaml", ".layout", ".txt", ".md", ".bmd", ".bvm" };
        }

        protected void InitializeCaches()
        { 
            foreach (string ext in FileExtensions)
            {
                FileCachesByExtension.AddMissing(ext, CreateCache(ext));
            }
        }

        ConcurrentDictionary<string, byte[]> _pageMinCache;
        object _pageMinCacheLock = new object();
        protected ConcurrentDictionary<string, byte[]> MinCache
        {
            get
            {
                return _pageMinCacheLock.DoubleCheckLock(ref _pageMinCache, () => new ConcurrentDictionary<string, byte[]>());
            }
        }

        ConcurrentDictionary<string, byte[]> _zippedPageMinCache;
        object _zippedPageMinCacheLock = new object();
        protected ConcurrentDictionary<string, byte[]> ZippedMinCache
        {
            get
            {
                return _zippedPageMinCacheLock.DoubleCheckLock(ref _zippedPageMinCache, () => new ConcurrentDictionary<string, byte[]>());
            }
        }

        /// <summary>
        /// Un-cache the specified file forcing it to be reloaded the next time it is 
        /// requested.
        /// </summary>
        /// <param name="file"></param>
        public void UncacheFile(FileInfo file)
        {
            Task.Run(() =>
            {
                string extension = Path.GetExtension(file.FullName).ToLower();
                if (FileCachesByExtension.ContainsKey(extension))
                {
                    FileCachesByExtension[extension].Remove(file);
                }
                foreach(AppContentResponder appContent in AppContentResponders.Values.ToArray())
                {
                    appContent.UncacheFile(file);
                }
            });
        }

        /// <summary>
        /// The server content root path, not to be confused with the 
        /// application root which should be [Root]\apps\[appName]
        /// </summary>
        public string Root
        {
            get
            {
                return ServerRoot.Root;
            }
        }

        public override bool MayRespond(IHttpContext context)
        {
            return !WillIgnore(context);
        }

        AppConf[] _appConfs;
        public AppConf[] AppConfigs
        {
            get
            {
                if(_appConfs == null)
                {
                    _appConfs = BamConf?.AppConfigs;
                }
                return _appConfs ?? new AppConf[] { };
            }
            set
            {
                _appConfs = value;
            }
        }

        public AppConf[] AppsToServe => BamConf?.AppsToServe;

        Dictionary<string, AppContentResponder> _appContentResponders;
        public Dictionary<string, AppContentResponder> AppContentResponders
        {
            get
            {
                if (_appContentResponders == null)
                {
                    _appContentResponders = new Dictionary<string, AppContentResponder>();
                }

                return _appContentResponders;
            }
        }

        public bool IsAppsInitialized
        {
            get;
            private set;
        }

        /// <summary>
        /// The event that fires when templates are being initialized.
        /// This occurs after file system initialization
        /// </summary>
        public event Action<ContentResponder> CommonTemplateRendererInitializing;

        /// <summary>
        /// The event that fires when templates have completed initialization.
        /// </summary>
        public event Action<ContentResponder> CommonTemplateRendererInitialized;

        protected internal void OnCommonTemplateRendererInitializing()
        {
            CommonTemplateRendererInitializing?.Invoke(this);
        }

        protected internal void OnCommonTemplateRendererInitialized()
        {
            CommonTemplateRendererInitialized?.Invoke(this);
        }

        [Inject]
        public ITemplateManager CommonTemplateManager
        {
            get;
            set;
        }

        /// <summary>
        /// Subscribe to the initialization related events
        /// of this ContentResponder and its ApplicationResponders.
        /// </summary>
        /// <param name="logger"></param>
        public override void Subscribe(ILogger logger)
        {            
            if (!IsSubscribed(logger))
            {
                base.Subscribe(logger);
                string className = typeof(ContentResponder).Name;
                this.AppContentRespondersInitializing += (c) =>
                {
                    logger.AddEntry("{0}::AppContentRespondersInitializ(ING)", className);
                };
                this.AppContentRespondersInitialized += (c) =>
                {
                    logger.AddEntry("{0}::AppContentRespondersInitializ(ED)", className);
                };
                this.AppContentResponderInitializing += (c, a) =>
                {
                    logger.AddEntry("{0}::AppContentResponderInitializ(ING):{1}", className, a.Name);
                };
                this.AppContentResponderInitialized += (c, a) =>
                {
                    logger.AddEntry("{0}::AppContentResponderInitializ(ED):{1}", className, a.Name);
                };
                this.Initializing += (c) =>
                {
                    logger.AddEntry("{0}::Initializ(ING)", className);
                };
                this.Initialized += (c) =>
                {
                    logger.AddEntry("{0}::Initializ(ED)", className);
                };
                this.CommonTemplateRendererInitializing += (c) =>
                {
                    logger.AddEntry("{0}::TemplatesInitializ(ING)", className);
                };
                this.CommonTemplateRendererInitialized += (c) =>
                {
                    logger.AddEntry("{0}::TemplatesInitializ(ED)", className);
                };
            }
        }
                
        protected virtual void SetBaseIgnorePrefixes()
        {
            AddIgnorPrefix("dao");
            AddIgnorPrefix("serviceproxy");
            AddIgnorPrefix("api");
            AddIgnorPrefix("bam");
            AddIgnorPrefix("meta");
            AddIgnorPrefix("get");
            AddIgnorPrefix("post");
            AddIgnorPrefix("securechannel");
        }

        protected internal void InitializeCommonTemplateRenderer()
        {
            OnCommonTemplateRendererInitializing();

            CommonTemplateManager = new CommonDustRenderer(this);

            OnCommonTemplateRendererInitialized();
        }

        public event Action<ContentResponder> AppContentRespondersInitializing;
        public event Action<ContentResponder> AppContentRespondersInitialized;

        protected internal void OnAppContentRespondersInitializing()
        {
            AppContentRespondersInitializing?.Invoke(this);
        }

        public event Action<ContentResponder, AppConf> AppContentResponderInitializing;
        public event Action<ContentResponder, AppConf> AppContentResponderInitialized;

        protected internal void OnAppContentResponderInitializing(AppConf appConf)
        {
            AppContentResponderInitializing?.Invoke(this, appConf);
        }

        protected internal void OnAppContentResponderInitialized(AppConf appConf)
        {
            AppContentResponderInitialized?.Invoke(this, appConf);
        }

        protected internal void OnAppRespondersInitialized()
        {
            AppContentRespondersInitialized?.Invoke(this);
        }

        object _initAppsLock = new object();
        /// <summary>
        /// Initialize all the AppContentResponders for the 
        /// apps found in the ~s:/apps folder
        /// </summary>
        protected internal void InitializeAppResponders()
        {
            OnAppContentRespondersInitializing();
            lock (_initAppsLock)
            {
                if (!IsAppsInitialized)
                {
                    InitializeHostAppMap(ContentRoot, AppConfigs ?? BamConf.AppConfigs);
                    InitializeAppResponders(AppsToServe);
                    AppConfigs = AppConfigs ?? BamConf.AppConfigs;
                    IsAppsInitialized = true;
                }
            }
            OnAppRespondersInitialized();
        }

        [Verbosity(LogEventType.Information)]
        public event EventHandler FileUploading;
        [Verbosity(LogEventType.Information)]
        public event EventHandler FileUploaded;
        
        private void InitializeAppResponders(AppConf[] configs)
        {
            configs.Each(appConf =>
            {
                OnAppContentResponderInitializing(appConf);
                Logger.RestartLoggingThread();
                AppContentResponder appContentResponder = new AppContentResponder(this, appConf)
                {
                    Logger = Logger
                };
                string appName = appConf.Name.ToLowerInvariant();
                IApplicationTemplateManager applicationTemplateManager = ApplicationServiceRegistry.Construct<AppHandlebarsRenderer>(appContentResponder);
                AppPageRendererManager pageRendererManager = new AppPageRendererManager(appContentResponder, ApplicationServiceRegistry.Get<ITemplateManager>(), applicationTemplateManager);
                Subscribers.Each(logger =>
                {
                    logger.RestartLoggingThread();
                    appContentResponder.Subscribe(logger);
                    pageRendererManager.Subscribe(logger);
                });
                appContentResponder.Initialize();
                appContentResponder.PageRenderer = pageRendererManager;
                appContentResponder.FileUploading += (o, a) => FileUploading?.Invoke(o, a);
                appContentResponder.FileUploaded += (o, a) => FileUploaded?.Invoke(o, a);
                appContentResponder.Responded += OnResponded;
                appContentResponder.DidNotRespond += OnDidNotRespond;
                appContentResponder.ContentNotFound += OnContentNotFound;
                appContentResponder.ContentResponder = this;
                appContentResponder.AppTemplateManager = applicationTemplateManager;
                ApplicationServiceRegistry.SetInjectionProperties(appContentResponder);
                AppContentResponders[appName] = appContentResponder;

                WriteHostProcessInfo(appConf);
                OnAppContentResponderInitialized(appConf);
            });
        }
        #region IResponder Members

        private void WriteHostProcessInfo(AppConf appConf)
        {
            HostProcessInfo processInfo = new HostProcessInfo(){ProcessMode = ProcessMode.Current};
            string relativeFilePath = "~/web-host.json";
            if (appConf.AppRoot.FileExists(out string fullPath, relativeFilePath))
            {
                File.Delete(fullPath);
            }
            appConf.AppRoot.WriteFile(relativeFilePath, processInfo.ToJson(true));
        }
        
        /// <summary>
        /// If true, TryRespond will send 404 and close the connection
        /// if no content is found.  Otherwise, nothing is done
        /// explicitly to close the connection or end the request.
        /// </summary>
        public bool EndResponse { get; set; }

        public override bool TryRespond(IHttpContext context)
        {
            return TryRespond(context, EndResponse);
        }

        public bool TryRespond(IHttpContext context, bool endResponse = false)
        {
            try
            {
                if (Etags.CheckEtags(context))
                {
                    return true;
                }

                if (!IsInitialized)
                {
                    Initialize();
                }

                IRequest request = context.Request;
                IResponse response = context.Response;
                Session.Init(context);
                SecureSession.Init(context);

                bool handled = false;
                string relativePathFromUrl = request.Url.AbsolutePath;
                string commonPath = Path.Combine("/common", relativePathFromUrl.TruncateFront(1));

                byte[] content = new byte[] { };
                string appName = ResolveApplicationName(context);
                List<string> allCheckedPaths = new List<string>();
                if (AppContentResponders.ContainsKey(appName))
                {
                    handled = AppContentResponders[appName].TryRespond(context, out string[] checkedPaths);
                    allCheckedPaths.AddRange(checkedPaths);
                }

                if (!handled && !ShouldIgnore(relativePathFromUrl))
                {
                    bool exists = CommonContentLocator.Locate(relativePathFromUrl, out string absoluteFileSystemPath, out string[] checkedPaths);
                    allCheckedPaths.AddRange(checkedPaths);
                    if (!exists)
                    {
                        exists = ServerRoot.FileExists(relativePathFromUrl, out absoluteFileSystemPath);
                        allCheckedPaths.Add(absoluteFileSystemPath);
                    }
                    if (!exists)
                    {
                        exists = ServerRoot.FileExists(commonPath, out absoluteFileSystemPath);
                        allCheckedPaths.Add(absoluteFileSystemPath);
                    }

                    if (exists)
                    {
                        string ext = Path.GetExtension(absoluteFileSystemPath);
                        if (FileCachesByExtension.ContainsKey(ext))
                        {
                            FileCache cache = FileCachesByExtension[ext];
                            if (ShouldZip(request))
                            {
                                SetGzipContentEncodingHeader(response);
                                content = cache.GetZippedContent(absoluteFileSystemPath);
                            }
                            else
                            {
                                content = cache.GetContent(absoluteFileSystemPath);
                            }
                            handled = true;
                            Etags.SetLastModified(response, request.Url.ToString(), new FileInfo(absoluteFileSystemPath).LastWriteTime);
                        }
                    }

                    if (handled)
                    {
                        SetContentType(response, relativePathFromUrl);
                        Etags.Set(response, request.Url.ToString(), content);
                        WriteResponse(response, content);
                        OnResponded(context);
                    }
                    else
                    {
                        LogContentNotFound(relativePathFromUrl, appName, allCheckedPaths.ToArray());
                        OnContentNotFound(this, context, allCheckedPaths.ToArray());
                        OnDidNotRespond(context);
                    }
                }

                if(!handled && endResponse)
                {
                    SendResponse(response, "Not Found", 404);
                }
                return handled;
            }
            catch (Exception ex)
            {
                Logger.AddEntry("An error occurred in {0}.{1}: {2}", ex, this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex.Message);
                OnDidNotRespond(context);
                return false;
            }
        }
        
        private string ResolveApplicationName(IHttpContext context)
        {
            if (HostAppMappings.ContainsKey(context.Request.Url.Host))
            {
                return HostAppMappings[context.Request.Url.Host].AppName;
            }
            return ApplicationNameResolver.ResolveApplicationName(context);
        }

        protected void LogContentNotFound(string path, string appName, string[] checkedPaths)
        {
            // Get the service names for the specified appName to determine whether it is
            // worth logging that this request was not handled.  If the content was not 
            // found because the request was intended for a different responder then 
            // no log entry should be made.
            string[] svcNames = BamConf?.Server?.ServiceProxyResponder?.AppServices(appName).Select(s => s.ToLowerInvariant()).ToArray();
            List<string> serviceNames = new List<string>();
            if(svcNames != null)
            {
                serviceNames.AddRange(svcNames);
            }
            string[] splitPath = path.DelimitSplit("/", "\\");
            string firstPart = splitPath.Length > 0 ? splitPath[0] : path;

            if(!ShouldIgnore(path) && !serviceNames.Contains(firstPart.ToLowerInvariant()))
            {
                StringBuilder checkedPathString = new StringBuilder();
                checkedPaths.Each(p =>
                {
                    checkedPathString.AppendLine(p);
                });

                Logger.AddEntry(
                  "App[{0}]::Path='{1}'::Content Not Found\r\nChecked Paths\r\n{2}",
                  LogEventType.Warning,
                  appName,
                  path,
                  checkedPathString.ToString()
                );
            }
        }
        
        static HashSet<string> _cachedScripts = new HashSet<string>();
        static object _cachedScriptLock = new object();
        protected internal void SetScriptCache(
            string path, string script)
        {
            if (!_cachedScripts.Contains(path))
            {
                lock (_cachedScriptLock)
                {
                    if (!_cachedScripts.Contains(path))
                    {
                        _cachedScripts.Add(path);
                    }
                    else
                    {
                        return;
                    }
                }

                Logger.AddEntry("Minification of ({0}) STARTED", LogEventType.Information, path);
                script.MinifyAsync().ContinueWith(t =>
                {
                    MinifyResult compression = t.Result;
                    if (compression.Success)
                    {
                        Logger.AddEntry("Minification of ({0}) COMPLETED", LogEventType.Information, path);
                        byte[] minBytes = Encoding.UTF8.GetBytes(compression.MinScript);
                        SetMinCacheBytes(path, minBytes);

                        string fileName = Path.GetFileName(path);
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
                        string pathWithoutFileName = path.TruncateFront(fileName.Length);
                        string minPath = Path.Combine(pathWithoutFileName, "{0}.min.js"._Format(fileNameWithoutExtension));
                        SetMinCacheBytes(minPath, minBytes);

                        Logger.AddEntry("GZipping the minified bytes of ({0}) STARTED", LogEventType.Information, path);
                        minBytes.GZipAsync().ContinueWith(g =>
                        {
                            Logger.AddEntry("GZipping the minified bytes of ({0}) COMPLETED", LogEventType.Information, path);
                            byte[] zippedMinBytes = g.Result;
                            SetZippedMinCacheBytes(minPath, zippedMinBytes);
                        });
                    }
                    else
                    {
                        string message = compression.Exception != null ? compression.Exception.Message : string.Empty;
                        string stack = string.Empty;
                        if (!string.IsNullOrEmpty(compression.Exception.StackTrace))
                        {
                            stack = compression.Exception.StackTrace;
                        }
                        Logger.AddEntry("Compression of ({0}) failed: {1}\r\n{2}", LogEventType.Warning, path, message, stack);
                    }
                });

                Task.Run(() =>
                {
                    byte[] scriptBytes = Encoding.UTF8.GetBytes(script);
                    SetCacheBytes(path, scriptBytes);
                    Logger.AddEntry("GZipping the bytes of ({0}) STARTED", LogEventType.Information, path);
                    scriptBytes.GZipAsync().ContinueWith(g =>
                    {
                        Logger.AddEntry("GZipping the minified bytes of ({0}) COMPLETED", LogEventType.Information, path);
                        SetZippedCacheBytes(path, g.Result);
                    });
                });
            }          
        }
        #endregion

        public override bool IsInitialized => IsAppsInitialized;

        public override void Initialize()
        {
            if (!IsInitialized)
            {
                OnInitializing();
                InitializeCommonTemplateRenderer();
                InitializeAppResponders();
                AppMetaInitializer = new AppMetaInitializer(this);
                OnInitialized();
            }
        }

        public event Action<ContentResponder> Initializing;

        protected void OnInitializing()
        {
            Initializing?.Invoke(this);
        }

        public event Action<ContentResponder> Initialized;
        protected void OnInitialized()
        {
            Initialized?.Invoke(this);
        }

        protected FileCache CreateCache(string fileExtension)
        {
            if (fileExtension.Equals(".js"))
            {
                return new JsFileCache(); 
            }
            else if (fileExtension.In(TextFileExtensions))
            {
                return new TextFileCache(fileExtension);
            }
            else
            {
                return new BinaryFileCache();
            }
        }

        static object cacheLock = new object();
        static object zippedCacheLock = new object();
        static object minCacheLock = new object();
        static object zippedMinCacheLock = new object();
        private void SetCacheBytes(string path, byte[] content)
        {
            Cache.AddOrUpdate(path, content, (s, b) => content);
        }
        private void SetZippedCacheBytes(string path, byte[] content)
        {
            ZippedCache.AddOrUpdate(path, content, (s, b) => content);
        }
        private void SetMinCacheBytes(string path, byte[] content)
        {
            MinCache.AddOrUpdate(path, content, (s, b) => content);
        }

        private void SetZippedMinCacheBytes(string path, byte[] content)
        {
            ZippedCache.AddOrUpdate(path, content, (s, b) => content);
        }
        
        private void InitializeHostAppMap(string contentRoot, AppConf[] appConfigs)
        {
            string jsonFile = Path.Combine(contentRoot, "apps", HostAppMapFile);
            HashSet<HostAppMap> temp = new HashSet<HostAppMap>();
            foreach (AppConf appConf in appConfigs)
            {
                temp.Add(new HostAppMap { Host = appConf.Name, AppName = appConf.Name });
            }
            if (File.Exists(jsonFile))
            {
                HostAppMap[] fromFile = jsonFile.FromJsonFile<HostAppMap[]>();
                if (fromFile != null)
                {
                    foreach (HostAppMap mapping in fromFile)
                    {
                        temp.Add(mapping);
                    }
                }
            }
            temp.ToJsonFile(jsonFile);
            Dictionary<string, HostAppMap> mappings = new Dictionary<string, HostAppMap>();
            foreach (HostAppMap hostAppMap in temp)
            {
                mappings.Add(GetNextKey(hostAppMap.Host, mappings), hostAppMap);
            }

            HostAppMappings = mappings;
        }

        private string GetNextKey(string key, Dictionary<string, HostAppMap> mappings)
        {
            string result = key;
            int num = 0;
            bool log = false;
            while (mappings.ContainsKey(result))
            {
                log = true;
                num++;
                result = $"{Path.GetFileNameWithoutExtension(result)}{num}";
            }

            if (log)
            {
                Logger.Warning("Multiple applications configured to use hostname ({0}): AppName=({1}) will use ({2}) for this process", key, mappings[key].AppName, result);
            }

            return result;
        }
    }
}
