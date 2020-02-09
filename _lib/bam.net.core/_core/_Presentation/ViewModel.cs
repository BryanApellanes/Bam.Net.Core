using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.CoreServices.ApplicationRegistration.Data;
using Bam.Net.Server;
using Bam.Net.Services.Hosts;
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
        
        public string ViewModelId { get; set; }

        public ViewModelUrl Url { get; set; }

        public IEnumerable<object> State { get; set; }

        public AppContentResponder AppContentResponder { get; internal set; }

        private ApplicationModel _applicationModel;

        /// <summary>
        /// The ApplicationModel.  This property is set when the current ViewModel
        /// is set as the ViewModel for a PageModel.
        /// </summary>
        public ApplicationModel Application
        {
            get => _applicationModel;
            internal set
            {
                _applicationModel = value;
                Url = new ViewModelUrl
                {
                    OrganizationName = _applicationModel?.Organization?.Name ?? ApplicationDiagnosticInfo.PublicOrganization,
                    ApplicationName = _applicationModel?.ApplicationName ?? ApplicationDiagnosticInfo.UnknownApplication,
                    HostName = Machine.Current.DnsName
                };
            }
        }
        
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

    }
}
