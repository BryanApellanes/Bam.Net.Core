/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.CoreServices;
using Bam.Net.ServiceProxy;
using Bam.Net.UserAccounts.Data;
using Bam.Net.Server;

namespace Bam.Net.CoreServices
{
    // TODO: fully implement this
    [Proxy("authSvc")]
    [ServiceSubdomain("auth")]
    public class AuthService: ApplicationProxyableService // This is not fully implemented
    {
        [Exclude]
        public override object Clone()
        {
            AuthService clone = new AuthService();
            clone.CopyProperties(this);
            clone.CopyEventHandlers(this);
            return clone;
        }

        public IHttpContext HttpContext
        {
            get;
            set;
        }

        public virtual void SetToken(string accessToken)
        {
            Session["accessToken"] = accessToken;            
        }        
    }
}
