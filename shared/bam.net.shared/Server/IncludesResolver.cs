using Bam.Net.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bam.Net.Server
{
    public class IncludesResolver : IIncludesResolver
    {
        public IncludesResolver(BamServer server)
        {
            BamServer = server;
        }

        public BamServer BamServer { get; set; }

        public Includes ResolveApplicationIncludes(string applicationName, string contentRoot)
        {
            Includes result = new Includes();
            result = AddAppIncludes(result, Path.Combine(contentRoot, "apps", applicationName, "include.yml"));
            result = AddAppIncludes(result, Path.Combine(contentRoot, "app", applicationName, "include.yaml"));
            result = AddAppIncludes(result, Path.Combine(contentRoot, "app", applicationName, "includes.yml"));
            result = AddAppIncludes(result, Path.Combine(contentRoot, "app", applicationName, "includes.yaml"));
            return result;
        }
        
        public Includes ResolveCommonIncludes(string contentRoot)
        {
            Includes result = new Includes();
            foreach (string commonPath in GetCommonPaths(contentRoot))
            {
                result = AddCommonIncludes(result, commonPath);
            }
            return result;
        }
        
        private Includes AddCommonIncludes(Includes includes, string path)
        {
            if (File.Exists(path))
            {
                includes = includes.Combine(path.FromYamlFile<Includes>());
                includes.FoundCommonPaths.Add(path);
            }

            return includes;
        }

        private string[] GetCommonPaths(string contentRoot)
        {
            return new string[]
            {
                Path.Combine(contentRoot, "include.yml"),
                Path.Combine(contentRoot, "include.yaml"),
                Path.Combine(contentRoot, "includes.yml"),
                Path.Combine(contentRoot, "includes.yaml"),
                
                Path.Combine(contentRoot, "common", "include.yml"),
                Path.Combine(contentRoot, "common", "include.yaml"),
                Path.Combine(contentRoot, "common", "includes.yml"),
                Path.Combine(contentRoot, "common", "includes.yaml"),
                
                Path.Combine(contentRoot, "apps", "include.yml"),
                Path.Combine(contentRoot, "apps", "include.yaml"),
                Path.Combine(contentRoot, "apps", "includes.yml"),
                Path.Combine(contentRoot, "apps", "includes.yaml"),
            };
        }
        
        private Includes AddAppIncludes(Includes includes, string path)
        {
            if (File.Exists(path))
            {
                includes = includes.Combine(path.FromYamlFile<Includes>());
                includes.FoundAppPaths.Add(path);
            }

            return includes;
        }
    }
}
