using Bam.Net.ServiceProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Bam.Net.Server;

namespace Bam.Net.CoreServices.Auth
{
    /// <summary>
    /// Request filter that ensures that the user is logged in,
    /// the request is for the current application and the user is
    /// a part of the organization of the current application.
    /// </summary>
    public class AuthenticatedAttribute : RequestFilterAttribute
    {
        public AuthenticatedAttribute()
        {
        }

        public override bool RequestIsAllowed(ExecutionRequest request, out string failureMessage)
        {
            failureMessage = null;
            ApplicationProxyableService service = request.Instance as ApplicationProxyableService;
            if(service == null)
            {
                MethodInfo method = request.MethodInfo;
                string messageFormat = $"{nameof(AuthenticatedAttribute)} adorned invalid type, must be placed on ApplicationProxyableService or derivative";
                failureMessage = string.Format(messageFormat, method.DeclaringType.Name, method.Name);
                request.Logger.Error(messageFormat, method.DeclaringType.Name, method.Name);
                return false;
            }

            if (!service.UserIsLoggedIn())
            {
                request.Logger.Error("Request-Id: {0}, User not logged in", request.Request.GetRequestId());
                return false;
            }

            if (!service.RequestIsForCurrentApplication())
            {
                request.Logger.Error("Request-Id: {0}, Request is not for the current application", request.Request.GetRequestId());
                return false;
            }

            if (!service.UserIsInApplicationOrganization())
            {
                request.Logger.Error("Request-Id: {0}, Current user {1} is not in the application ({2}) organization ({3})", request.Request.GetRequestId(), service.CurrentUser.UserName, service.ClientApplication, service.ClientOrganization);
                return false;
            }
            return true;
        }
    }
}
