using Bam.Net.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Data.Repositories;
using System.IO;
using Bam.Net.CoreServices.Auth.Data;
using System.Collections;

namespace Bam.Net.CoreServices.Auth
{
    public class SupportedAuthProviders: IEnumerable<AuthProviderInfo>
    {
        List<AuthProviderInfo> _authProviderSettings;
        public SupportedAuthProviders()
        {
            _authProviderSettings = new List<AuthProviderInfo>();
        }
        
        public void AddProvider(AuthProviderInfo provider)
        {
            _authProviderSettings.Add(provider);
        }

        public void Save(string filePath)
        {
            _authProviderSettings.ToJsonFile(filePath);
        }

        public void Load(string filePath)
        {
            _authProviderSettings = filePath.FromJsonFile<List<AuthProviderInfo>>();
        }

        public static SupportedAuthProviders LoadFrom(string filePath)
        {
            SupportedAuthProviders result = new SupportedAuthProviders();
            result.Load(filePath);
            return result;
        }

        public static SupportedAuthProviders Get(IApplicationNameProvider appNameProvider)
        {
            string filePath = GetSettingsPath(appNameProvider);
            return LoadFrom(filePath);
        }

        public static string GetSettingsPath(IApplicationNameProvider appNameProvider)
        {
            string appName = appNameProvider.GetApplicationName();
            string filePath = Path.Combine(DataProvider.Current.AppDataDirectory, appName, $"{nameof(SupportedAuthProviders)}.json");
            return filePath;
        }

        public AuthProviderInfo this[string providerName]
        {
            get
            {
                return _authProviderSettings.FirstOrDefault(p => p.ProviderName.Equals(providerName));
            }
        }

        public IEnumerator<AuthProviderInfo> GetEnumerator()
        {
            return _authProviderSettings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _authProviderSettings.GetEnumerator();
        }
    }
}
