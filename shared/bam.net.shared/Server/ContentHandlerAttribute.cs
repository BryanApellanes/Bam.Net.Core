using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.Server
{
    /// <summary>
    /// Attribute used to adorn a static method that returns a byte[]
    /// and takes IHttpContext and Fs as arguments.  The method is
    /// registered as a content handler during content handler scanning
    /// done by AppContentResponder instances.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ContentHandlerAttribute: Attribute
    {
    }
}
