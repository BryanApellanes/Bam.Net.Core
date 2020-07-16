﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Server;

namespace Bam.Net.Server.PathHandlers
{
    public abstract class PathRoute
    {
        public string Route => string.Format("{Protocol}://{Domain}/{0}/{PathAndQuery}", HandlerName);

        public string HandlerName { get; set; }
        public string Protocol { get; set; }
        public string Domain { get; set; }
        public string PathAndQuery { get; set; }      
        public MethodRoute MethodRoute { get; set; }

        public bool IsMatch(string uri)
        {
            return IsMatch(uri, out PathRoute ignore);
        }
        
        public bool IsMatch(string uri, out PathRoute pathRoute)
        {
            pathRoute = ParseRoute(uri);
            return Route.NamedFormat(pathRoute).Equals(uri);
        }

        public virtual PathRoute ParseRoute(string uri)
        {
            RouteParser parser = new RouteParser(Route);
            PathRoute route = (PathRoute)parser.ParseRouteInstance(uri).ToInstance(this.GetType());
            route.ParseMethod();
            return route;
        }

        /// <summary>
        /// Returns true if Protocol Domain and PathAndQuery properties are set.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Protocol) && !string.IsNullOrEmpty(Domain) && !string.IsNullOrEmpty(PathAndQuery);
        }

        /// <summary>
        /// Parses the PathAndQuery property to populate the MethodRoute property.
        /// </summary>
        /// <returns></returns>
        public bool ParseMethod()
        {
            if (!IsValid())
            {
                return false;
            }
            MethodRoute = new MethodRoute();
            return MethodRoute.Parse(PathAndQuery);
        }
    }
}
