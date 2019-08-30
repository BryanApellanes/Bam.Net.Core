using System;
using System.Collections.Generic;
using System.IO;

namespace Bam.Net.CoreServices.AssemblyManagement
{
    public class PackageVersionResolver : IPackageVersionResolver
    {
        public PackageVersionResolver(NugetReferenceAssemblyResolver nugetReferenceAssemblyResolver)
        {
            NugetReferenceAssemblyResolver = nugetReferenceAssemblyResolver;
        }
     
        protected NugetReferenceAssemblyResolver NugetReferenceAssemblyResolver { get; set; }
        public string Preferred { get; set; }
        
        public string ResolveVersion(Type type)
        {
            string packageDirectory = NugetReferenceAssemblyResolver.ResolvePackageRootDirectory(type);
            
            string candidatePath = GetVersionDirectory(packageDirectory);

            if (!Directory.Exists(candidatePath))
            {
                throw new PackageVersionNotFoundException(type);
            }
            
            return new DirectoryInfo(candidatePath).Name;
        }

        public string ResolveVersion(string nameSpace, string name)
        {
            string packageDirectory = NugetReferenceAssemblyResolver.ResolvePackageRootDirectory(nameSpace, name);

            string candidatePath = GetVersionDirectory(packageDirectory);

            if (!Directory.Exists(candidatePath))
            {
                throw new PackageVersionNotFoundException($"{nameSpace}.{name}");
            }
            
            return new DirectoryInfo(candidatePath).Name;
        }
        
        private string GetVersionDirectory(string packageDirectory)
        {
            DirectoryInfo packageDirectoryInfo = new DirectoryInfo(packageDirectory);
            string candidatePath = string.Empty;
            if (!string.IsNullOrEmpty(Preferred))
            {
                candidatePath = Path.Combine(packageDirectoryInfo.FullName, Preferred);
            }

            if (string.IsNullOrEmpty(candidatePath))
            {
                List<DirectoryInfo> versionDirectories = new List<DirectoryInfo>(packageDirectoryInfo.GetDirectories());
                versionDirectories.Sort((x, y) => y.Name.CompareTo(x.Name));
                candidatePath = Path.Combine(packageDirectory, versionDirectories[0].Name);
            }

            return candidatePath;
        }
    }
}