﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.Server.PathHandlers
{
    public class BamPathRoute : PathRoute
    {
        public BamPathRoute()
        {
            HandlerName = "bam";
        }

        public static BamPathRoute Parse(string uri)
        {
            BamPathRoute route = new BamPathRoute();
            return (BamPathRoute)route.ParseRoute(uri);
        }
    }
}
