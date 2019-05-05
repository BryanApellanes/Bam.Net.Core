using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Services;

namespace Bam.Net.Server
{
    public class RequestRouter
    {
        public const string Route = "{Protocol}://{Domain}/$PathName/{PathAndQuery}";
        public RequestRouter() { }
        public RequestRouter(string pathName)
        {
            PathName = pathName;
        }

        public bool IsHomeRequest(string uri)
        {
            return IsHomeRequest(uri, out RequestRoute ignore);
        }

        public bool IsHomeRequest(string uri, out RequestRoute requestRoute)
        {
            HomeRoute homeRoute = new HomeRoute(uri);
            requestRoute = ToRequestRoute(uri);
            return homeRoute.IsValid;
        }
        
        /// <summary>
        /// The prefix of the path
        /// </summary>
        public string PathName { get; set; }


        public RequestRoute ToRequestRoute(string url)
        {
            return ToRequestRoute(new Uri(url));
        }

        public RequestRoute ToRequestRoute(Uri uri)
        {
            Dictionary<string, string> values = ToRouteValues(uri);

            RequestRoute route = new RequestRoute { PathName = PathName, OriginalUrl = uri };
            route.Protocol = values["Protocol"];
            route.Domain = values["Domain"];
            route.PathAndQuery = values["PathAndQuery"];
            route.ParsedValues = values;
            return route;
        }

        protected Dictionary<string, string> ToRouteValues(Uri uri)
        {
            RouteParser parser = new RouteParser(Route.Replace("$PathName", PathName));
            Dictionary<string, string> values = parser.ParseRouteInstance(uri.ToString());
            return values;
        }
    }
}
