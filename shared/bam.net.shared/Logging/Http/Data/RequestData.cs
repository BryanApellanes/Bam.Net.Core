using Bam.Net.Data.Repositories;
using Bam.Net.ServiceProxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net;
using System.IO;
using Bam.Net.Server;

namespace Bam.Net.Logging.Http.Data
{
    [Serializable]
    public class RequestData : KeyedRepoData
    {
        /// <summary>
        /// The value of the `x-request-id` header if present.
        /// </summary>
        [CompositeKey]
        public string RequestId { get; set; }
        public string AcceptTypes { get; set; }
        public string ContentEncoding { get; set; }
        public long ContentLength { get; set; }
        public string ContentType { get; set; }
        public virtual List<CookieData> Cookies { get; set; }
        public virtual List<HeaderData> Headers { get; set; }
        public string HttpMethod { get; set; }
        public byte[] InputStream { get; set; }

        public ulong UrlKey { get; set; }
        public virtual UriData Url { get; set; }

        public ulong UrlReferrerId { get; set; }
        public virtual UriData UrlReferrer { get; set; }
        public string UserAgent { get; set; }
        public string UserHostAddress { get; set; }
        public string UserHostName { get; set; }
        public string UserLanguages { get; set; } // defined on IRequest as a string[] array. comma delimit it for storage
        public string RawUrl { get; set; }

        public static RequestData FromRequest(IRequest request)
        {
            Args.ThrowIfNull(request, "request");
            
            RequestData requestData = new RequestData
            {
                RequestId = request.GetRequestId(),
                AcceptTypes = request.AcceptTypes == null ? string.Empty: string.Join(",",  request.AcceptTypes),
                ContentEncoding = request.ContentEncoding.ToString(),
                ContentLength = request.ContentLength64,
                ContentType = request.ContentType,
                HttpMethod = request.HttpMethod,
                Url = UriData.FromUri(request.Url),
                UrlReferrer = request.UrlReferrer == null ? null: UriData.FromUri(request.UrlReferrer),
                UserAgent = request.UserAgent,
                UserHostAddress = request.UserHostAddress,
                UserHostName = request.UserHostName,
                UserLanguages = request.UserLanguages == null ? string.Empty: string.Join(",", request.UserLanguages),
                RawUrl = request.RawUrl,
                Cookies = new List<CookieData>(),
                Headers = new List<HeaderData>()
            };
            foreach(Cookie cookie in request?.Cookies)
            {
                requestData.Cookies.Add(CookieData.FromCookie(cookie));
            }
            foreach(string key in request?.Headers.AllKeys)
            {
                requestData.Headers.Add(new HeaderData { Name = key, Value = request.Headers[key] });
            }
            MemoryStream inputStream = new MemoryStream();
            request?.InputStream?.CopyTo(inputStream);
            requestData.InputStream = inputStream.ToArray();
            return requestData;
        }
    }
}
