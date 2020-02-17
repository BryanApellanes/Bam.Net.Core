using System;
using System.Collections.Generic;
using Bam.Net.Automation;
using Bam.Net.Logging;

namespace Bam.Net.Application.Verbs
{
    public class Scan
    {
        static readonly Dictionary<Type, Scanner> _scanners;
        
        static Scan()
        {
            _scanners = new Dictionary<Type, Scanner>();
        }

        /// <summary>
        /// Executes the prepared function for the specified generic return type, if one was prepared with a call to the Prepare method.  If a scanner function is specified Prepare is called passing the specified scanner as a parameter before executing it to retrieve the result.
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="scanner"></param>
        /// <returns></returns>
        public static T HostFor<T>(string hostName)
        {
            if (_scanners.ContainsKey(typeof(T)))
            {
                return (T)_scanners[typeof(T)].GetResult(hostName);
            }

            return default(T);
        }

        /// <summary>
        /// Prepare the specified scanner.  The results of 
        /// </summary>
        /// <param name="hostScanner"></param>
        public static void SetHostScanFor<T>(Func<string, T> hostScanner)
        {
            if (!_scanners.ContainsKey(typeof(T)))
            {
                _scanners.AddMissing(typeof(T), new Scanner<T>() {Scan = hostScanner});
            }
            else
            {
                Args.Throw<InvalidOperationException>("Scanner for type ({0}) is already set.", typeof(T).FullName);
            }
        }
    }
}