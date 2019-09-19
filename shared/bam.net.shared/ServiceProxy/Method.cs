using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.ServiceProxy
{
    public static class Method
    {
        /// <summary>
        /// Returns true if the specified method will proxy AND it is adorned with the WebServiceAttribute.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="includeLocal"></param>
        /// <returns></returns>
        public static bool WillProxyWebService(this MethodInfo method, bool includeLocal = false)
        {
            return WillProxy(method, includeLocal) && method.HasCustomAttributeOfType<WebServiceAttribute>();
        }
        
        public static bool WillProxy(this MethodInfo method, bool includeLocal = false)
        {
            bool baseCheck = !method.Name.StartsWith("remove_") &&
                                   !method.Name.StartsWith("add_") &&
                                   method.MemberType == MemberTypes.Method &&
                                   !method.IsProperty() &&
                                   method.DeclaringType != typeof(object);
            bool hasExcludeAttribute = method.HasCustomAttributeOfType(out ExcludeAttribute attr);
            bool result = false;
            if (includeLocal)
            {
                result = hasExcludeAttribute ? (attr is LocalAttribute && baseCheck) : baseCheck;
            }
            else
            {
                result = !hasExcludeAttribute && baseCheck;
            }
            return result;
        }
    }
}
