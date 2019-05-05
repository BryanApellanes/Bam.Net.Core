using System;
using System.Collections.Generic;

namespace Bam.Net.Server
{
    public class HomeRoute
    {
        public HomeRoute(string url)
        {
            Uri = new Uri(url);
            Parse();
        }

        public bool IsValid
        {
            get { return !string.IsNullOrEmpty(Protocol) && !string.IsNullOrEmpty(Domain); }
        }

        Dictionary<string, string> values; 
        
        public Uri Uri { get; set; }
        public string Protocol  
        {
            get => values["Protocol"];
            set => values["Protocol"] = value;
        }
        
        public string Domain
        {
            get => values["Domain"];
            set => values["Domain"] = value;
        }

        private void Parse()
        {
            RouteParser parser = new RouteParser("{Protocol}://{Domain}");
            values = parser.ParseRouteInstance(Uri.ToString());
        }
    }
}