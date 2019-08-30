using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using GraphQL.Types;
using Lucene.Net.Analysis.Hunspell;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Operations;
using Org.BouncyCastle.Bcpg;

namespace Bam.Net.CoreServices.AssemblyManagement
{
    public class NugetReferenceAssemblyResolver : IReferenceAssemblyResolver
    {
        public NugetReferenceAssemblyResolver(string nugetFallbackRoot)
        {
            NugetFallbackRoot = nugetFallbackRoot;
            PackageVersionResolver = new PackageVersionResolver(this);
            NetStandardVersionResolver = new NetStandardVersionResolver();
        }

        private static Dictionary<OSNames, NugetReferenceAssemblyResolver> _nugetReferenceAssemblyResolvers;
        private static readonly object _nugetReferenceAssemblyResolversLock = new object();
        protected static Dictionary<OSNames, NugetReferenceAssemblyResolver> NugetReferenceAssemblyResolvers
        {
            get
            {
                return _nugetReferenceAssemblyResolversLock.DoubleCheckLock(ref _nugetReferenceAssemblyResolvers, () => new Dictionary<OSNames, NugetReferenceAssemblyResolver>()
                {
                    {OSNames.Windows, new ProfileReferenceAssemblyResolver()},
                    {OSNames.OSX, new NugetReferenceAssemblyResolver("/usr/local/share/dotnet/sdk/NugetFallbackFolder")},
                    {OSNames.Linux, new ProfileReferenceAssemblyResolver()}
                });
            }
        }
        
        public static NugetReferenceAssemblyResolver Windows => NugetReferenceAssemblyResolvers[OSNames.Windows];
        public static NugetReferenceAssemblyResolver OSX => NugetReferenceAssemblyResolvers[OSNames.OSX];
        public static NugetReferenceAssemblyResolver Linux => NugetReferenceAssemblyResolvers[OSNames.Linux];

        public static NugetReferenceAssemblyResolver ForOs(OSNames os)
        {
            return NugetReferenceAssemblyResolvers[os];
        }
        
        protected string NugetFallbackRoot { get; set; }

        protected IPackageVersionResolver PackageVersionResolver { get; set; }
        protected INetStandardVersionResolver NetStandardVersionResolver { get; set; }
        
        public Assembly ResolveReferenceAssembly(Type type)
        {
            return Assembly.LoadFrom(ResolveReferenceAssemblyPath(type));
        }

        public string ResolveReferenceAssemblyPath(Type type)
        {
            string packageDirectoryRoot = ResolvePackageRootDirectory(type);

            string packageVersion = PackageVersionResolver.ResolveVersion(type);

            string typeNamespace = type.Namespace;
            string typeName = type.Name;
            
            string packagePath = GetPackagePath(packageDirectoryRoot, packageVersion, typeNamespace, typeName);

            if (!File.Exists(packagePath))
            {
                throw new ReferenceAssemblyNotFoundException(type);
            }

            return packagePath;
        }

        public string ResolveReferenceAssemblyPath(string nameSpace, string typeName)
        {
            string packageDirectoryRoot = ResolvePackageRootDirectory(nameSpace, typeName);
            
            string packageVersion = PackageVersionResolver.ResolveVersion(nameSpace, typeName);

            string packagePath = GetPackagePath(packageDirectoryRoot, packageVersion, nameSpace, typeName);

            if (!File.Exists(packagePath))
            {
                throw new ReferenceAssemblyNotFoundException($"{nameSpace}.{typeName}");
            }

            return packagePath;
        }

        public string ResolveSystemRuntimePath()
        {
            if (OSInfo.Current == OSNames.Windows)
            {
                return typeof(object).Assembly.GetFilePath();
            }
            
            string packageDirectoryRoot = ResolvePackageRootDirectory("System", "Runtime");
            
            string packageVersion = PackageVersionResolver.ResolveVersion("System", "Runtime");

            string versionDirectoryRoot = Path.Combine(packageDirectoryRoot, packageVersion);

            string packagePath = GetPackagePath(packageDirectoryRoot, packageVersion, "System", "Runtime");

            if (!File.Exists(packagePath))
            {
                throw new ReferenceAssemblyNotFoundException($"System.Runtime");
            }

            return packagePath;
        }

        public string ResolvePackageRootDirectory(Type type)
        {
            DirectoryInfo nugetRoot = new DirectoryInfo(NugetFallbackRoot);
            string packageDirectoryRoot = Path.Combine(nugetRoot.FullName, $"{type.Namespace}".ToLowerInvariant());

            if (!Directory.Exists(packageDirectoryRoot))
            {
                packageDirectoryRoot = Path.Combine(nugetRoot.FullName, $"{type.Namespace}.{type.Name}".ToLowerInvariant());
            }

            if (!Directory.Exists(packageDirectoryRoot))
            {
                throw new ReferenceAssemblyNotFoundException(type);
            }

            return packageDirectoryRoot;
        }

        public string ResolvePackageRootDirectory(string typeNamespace, string typeName)
        {
            DirectoryInfo nugetRoot = new DirectoryInfo(NugetFallbackRoot);
            string packageDirectoryRoot = Path.Combine(nugetRoot.FullName, $"{typeNamespace}");

            if (!Directory.Exists(packageDirectoryRoot))
            {
                packageDirectoryRoot = Path.Combine(nugetRoot.FullName, $"{typeNamespace}.{typeName}");
            }

            if (!Directory.Exists(packageDirectoryRoot))
            {
                throw new ReferenceAssemblyNotFoundException($"{typeNamespace}.{typeName}");
            }

            return packageDirectoryRoot;
        }
        
        private string GetPackagePath(string packageDirectoryRoot, string packageVersion, string typeNamespace, string typeName)
        {
            string versionDirectoryRoot = Path.Combine(packageDirectoryRoot, packageVersion);

            string refDirectory = Path.Combine(versionDirectoryRoot, "ref");

            string netStandardVersion = NetStandardVersionResolver.ResolveVersion(new DirectoryInfo(refDirectory));

            string packageDirectory = Path.Combine(refDirectory, netStandardVersion);

            string packagePath = Path.Combine(packageDirectory, $"{typeNamespace}.dll".ToLowerInvariant());

            if (!File.Exists(packagePath))
            {
                packagePath = Path.Combine(packageDirectory, $"{typeNamespace}.{typeName}.dll".ToLowerInvariant());
            }

            return packagePath;
        }
    }
}