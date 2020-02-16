using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.Web
{
    public static class Headers
    {
        public static string ProcessSrc => "X-Bam-Process-Src";

        public static string ProcessMode => "X-Bam-Process-Mode";

        public static string ApplicationName => "X-Bam-AppName";
        public static string SecureSession => "X-Bam-Sps-Session";
        public static string ValidationToken => "X-Bam-Validation-Token";
        public static string Signature => "X-Bam-Signature";
        public static string Nonce => "X-Bam-Timestamp";
        public static string Padding => "X-Bam-Padding";

        /// <summary>
        /// Header used to prove that the client knows the shared secret by using 
        /// it to create a hash value that this header is set to.
        /// </summary>
        public static string KeyToken => "X-Bam-Key-Token";

        /// <summary>
        /// Header used to request a specific responder on the server
        /// handle a given request.
        /// </summary>
        public static string Responder => "X-Bam-Responder";
    }
}
