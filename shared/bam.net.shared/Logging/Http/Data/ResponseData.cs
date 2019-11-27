using System;
using System.Collections.Generic;
using System.Net;
using Bam.Net.Data.Repositories;
using Bam.Net.Server;
using Bam.Net.ServiceProxy;
using CsQuery.Implementation;

namespace Bam.Net.Logging.Http.Data
{
    [Serializable]
    public class ResponseData: KeyedRepoData
    {
        public static ResponseData FromResponse(IHttpContext context, string[] checkedPaths = null)
        {
            return FromResponse(context.Request.GetRequestId(), context.Response, checkedPaths);
        }
        
        public static ResponseData FromResponse(string requestId, IResponse response, string[] checkedPaths = null)
        {
            ResponseData responseData = new ResponseData
            {
                RequestId =  requestId,
                CheckedPaths = checkedPaths != null ? string.Join(", ", checkedPaths) : "",
                ContentLength64 =  response.ContentLength64,
                ContentType =  response.ContentType,
                RedirectLocation = response.RedirectLocation,
                StatusCode =  response.StatusCode,
                StatusDescription = response.StatusDescription,
                Cookies =  new List<CookieData>(),
                Headers = new List<HeaderData>()
            };
            foreach (Cookie cookie in response?.Cookies)
            {
                responseData.Cookies.Add(CookieData.FromCookie(cookie));
            }

            foreach (string key in response?.Headers)
            {
                responseData.Headers.Add(new HeaderData {Name = key, Value = response.Headers[key]});
            }
            return responseData;
        }
        
        [CompositeKey]
        public string RequestId { get; set; }
        [CompositeKey]
        public string CheckedPaths { get; set; }
        public long ContentLength64 { get; set; }
        [CompositeKey]
        public string ContentType { get; set; }
        [CompositeKey]
        public string RedirectLocation { get; set; }
        [CompositeKey]
        public int StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public virtual List<CookieData> Cookies { get; set; }
        public virtual List<HeaderData> Headers { get; set; }
    }
}