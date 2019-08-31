
using System.Collections.Generic;
using System.IO;

namespace Bam.Net.CoreServices.AssemblyManagement
{
    public class NetStandardVersionResolver : INetStandardVersionResolver
    {
        public const string Prefix = "netstandard";
        
        public string ResolveVersion(DirectoryInfo packageRefRoot)
        {
            List<DirectoryInfo> candidateDirectories = new List<DirectoryInfo>(packageRefRoot.GetDirectories($"{Prefix}*"));
            if (candidateDirectories.Count > 0)
            {
                candidateDirectories.Sort((x,y)=> y.Name.CompareTo(x.Name));
                return candidateDirectories[0].Name;
            }

            return string.Empty;
        }
    }
}