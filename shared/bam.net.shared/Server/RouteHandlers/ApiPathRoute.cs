﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.Server.PathHandlers
{
    public class ApiPathRoute : PathRoute
    {
        public ApiPathRoute()
        {
            HandlerName = "api";
        }

        public static ApiPathRoute Parse(string uri)
        {
            ApiPathRoute route = new ApiPathRoute();
            return (ApiPathRoute)route.ParseRoute(uri);
        }
    }
}
