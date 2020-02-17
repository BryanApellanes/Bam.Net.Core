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
        

        public virtual PathRoute ParseRoute(string uri)
        {
            RouteParser parser = new RouteParser(Route);
            PathRoute route = (PathRoute)parser.ParseRouteInstance(uri).ToInstance(this.GetType());
            route.ParseMethod();
            return route;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Protocol) && !string.IsNullOrEmpty(Domain) && !string.IsNullOrEmpty(PathAndQuery);
        }

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
