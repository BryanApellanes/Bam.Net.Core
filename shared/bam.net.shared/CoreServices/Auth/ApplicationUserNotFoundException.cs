using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.CoreServices.Auth
{
    [Serializable]
    public class ApplicationUserNotFoundException: Exception
    {
        public ApplicationUserNotFoundException(string applicationName, string userName) : base($"Application user not found: Application={applicationName}, User={userName}")
        { }
    }
}
