using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using Bam.Net.ServiceProxy;
using NLog.Targets;

namespace Bam.Net.Server
{
    public static class RequestExtensions
    {
        private static string RequestIdHeader = "x-request-id";

        /// <summary>
        /// If the `x-request-id` header is not present then it is added.
        /// </summary>
        /// <param name="request"></param>
        public static void SetRequestId(this IRequest request)
        {
            if (!request.HasHeader(RequestIdHeader))
            {
                request.Headers.Add(RequestIdHeader, "bam-".RandomString(16));
            }
        }

        public static void SetRequestId(this HttpListenerContext context)
        {
            SetRequestId(context.Request);
        }

        public static void SetRequestId(this HttpListenerRequest request)
        {
            if (request == null)
            {
                return;
            }
            if (!request.HasRequestIdHeader())
            {
                request.Headers.Add(RequestIdHeader, "bam-".RandomLetters(32));
            }
        }
        public static string GetRequestId(this IRequest request)
        {
            if (request?.Headers == null)
            {
                return string.Empty;
            }

            return request.HasHeader(RequestIdHeader, out string requestId) ? requestId : string.Empty;
        }

        public static bool HasHeader(this IRequest request, string headerName)
        {
            return HasHeader(request, headerName, out string ignore);
        }

        public static bool HasHeader(this IRequest request, string headerName, out string headerValue)
        {
            headerValue = string.Empty;

            if (request?.Headers != null && !string.IsNullOrEmpty(request?.Headers[headerName]))
            {
                headerValue = request.Headers[headerName];
                return true;
            }

            return false;
        }

        public static string GetRequestId(this HttpListenerRequest request)
        {
            if (request.HasRequestIdHeader(out string requestId))
            {
                return requestId;
            }

            return string.Empty;
        }
        
        public static bool HasRequestIdHeader(this HttpListenerRequest request)
        {
            return HasRequestIdHeader(request, out string ignore);
        }
        
        public static bool HasRequestIdHeader(this HttpListenerContext context, out string requestId)
        {
            return HasRequestIdHeader(context.Request, out requestId);
        }

        public static bool HasRequestIdHeader(this HttpListenerRequest request, out string requestId)
        {
            return HasRequestIdHeader(request.Headers, out requestId);
        }
        
        public static bool HasRequestIdHeader(NameValueCollection webHeaderCollection, out string requestId)
        {
            requestId = webHeaderCollection.Get(RequestIdHeader).Or(string.Empty);
            return !string.IsNullOrEmpty(requestId);
        }
    }
}