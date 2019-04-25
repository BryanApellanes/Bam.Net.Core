using Bam.Net;
using Bam.Net.CoreServices;
using Bam.Net.Logging;

namespace Bam.Net.Services
{
    public class ServiceHostSettings
    {
        public ServiceHostSettings()
        {
            _hostName = "localhost";
            _port = 9100;
            Logger = Log.Default;
        }

        static ServiceHostSettings _current;
        static object _currentLock = new object();
        public static ServiceHostSettings Current
        {
            get { return _currentLock.DoubleCheckLock(ref _current, () => new ServiceHostSettings()); }
            set { _current = value; }
        }

        public ILogger Logger { get; set; }
        public ServiceRegistry ServiceRegistry { get; set; }
        
        string _hostName;
        public string HostName
        {
            get
            {
                return _hostName;
            }
            set
            {
                _hostName = value;
                SetProxyFactory();
            }
        }

        int _port;

        public int Port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
                SetProxyFactory();
            }
        }

        public T GetService<T>() where T: ProxyableService
        {
            return ProxyFactory.GetProxy<T>();
        }
        
        protected ProxyFactory ProxyFactory { get; private set; }
        
        private void SetProxyFactory()
        {
            ProxyFactory = new ProxyFactory(Workspace.ForProcess().Path("Proxies"), Logger, ServiceRegistry);
        }
    }
}