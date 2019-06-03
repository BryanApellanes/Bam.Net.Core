using System;
using System.Collections.Generic;
using Bam.Net.Automation;
using Bam.Net.Logging;

namespace Bam.Net.Application.Verbs
{
    public class Scan
    {
        static Dictionary<Type, Func<string, object>> _scanners;
        static Dictionary<string, object> _scanResults;
        
        static Scan()
        {
            _scanners = new Dictionary<Type, Func<string, object>>();
            _scanResults = new Dictionary<string, object>();
        }
        
        public static T For<T>(string hostName)
        {
            if (_scanResults.ContainsKey(hostName) && _scanResults[hostName] != null)
            {
                return (T)_scanResults[hostName];
            }
            
            if (_scanners.ContainsKey(typeof(T)))
            {
                object result = _scanners[typeof(T)](hostName);
                if (result is T response)
                {
                    _scanResults.AddMissing(hostName, response);
                    return response;
                }
                else
                {
                    Log.Error("The scanner for hostName ({0}) and type ({1}) returned an instance of ({2})", hostName, typeof(T).FullName,
                        result?.GetType().FullName);
                }
            }

            return default(T);
        }

        public static void Prepare<T>(Func<string, object> hostNameScanner)
        {
            if (!_scanners.ContainsKey(typeof(T)))
            {
                _scanners.AddMissing(typeof(T), hostNameScanner);
            }
            else
            {
                Args.Throw<InvalidOperationException>("Scanner for type ({0}) is already set.", typeof(T).FullName);
            }
        }
    }
}