using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Engines;

namespace Bam.Net.Presentation
{
    public class ViewModel<AP> : ViewModel
    {
        public new AP ActionProvider { get; set; }
    }
    
    public class ViewModel
    {
        public string Name { get; set; }

        public IEnumerable<object> State { get; set; }

        /// <summary>
        /// Gets or sets the action provider.  This should be an 
        /// instance of an object whose class definition has the 
        /// ProxyAttribute applied.
        /// </summary>
        /// <value>
        /// The action provider.
        /// </value>
        public dynamic ActionProvider { get; set; }

        public void Execute(string methodName)
        {
            Execute(methodName, State.ToArray());
        }
        
        public void Execute(string methodName, params object[] args)
        {
            ActionProvider?.Invoke(methodName, args);
        }
        
        internal string ViewModelId { get; set; }
    }
}
