
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bam.Net.CoreServices.AssemblyManagement
{
    public abstract class ReferenceAssemblyResolver: IReferenceAssemblyResolver
    {
        private static Dictionary<OSNames, IReferenceAssemblyResolver> _referenceAssemblyResolvers;
        static readonly object _referenceAssemblyResolversLock = new object();

        private static Dictionary<OSNames, IReferenceAssemblyResolver> ReferenceAssemblyResolvers
        {
            get
            {
                return _referenceAssemblyResolversLock.DoubleCheckLock<Dictionary<OSNames, IReferenceAssemblyResolver>>(ref _referenceAssemblyResolvers, ()=> new Dictionary<OSNames, IReferenceAssemblyResolver>()
                {
                    {OSNames.Windows, new RuntimeSettingsConfigReferenceAssemblyResolver()},
                    {OSNames.OSX, new OsxReferenceAssemblyResolver()},
                    {OSNames.Linux, new LinuxReferenceAssemblyResolver()}
                });
            }
        }

        public static IReferenceAssemblyResolver Current => ReferenceAssemblyResolvers[OSInfo.Current];

        public virtual Assembly ResolveReferenceAssembly(Type type)
        {
            return Assembly.Load(ResolveReferenceAssemblyPath(type));
        }

        public virtual string ResolveReferenceAssemblyPath(Type type)
        {
            return ReferenceAssemblyResolvers[OSInfo.Current].ResolveReferenceAssemblyPath(type);
        }

        public virtual string ResolveReferenceAssemblyPath(string nameSpace, string typeName)
        {
            return ReferenceAssemblyResolvers[OSInfo.Current].ResolveReferenceAssemblyPath(nameSpace, typeName);
        }

        public virtual string ResolveSystemRuntimePath()
        {
            return RuntimeSettings.GetSystemRuntimePath();
        }

        public static string GetReferenceAssemblyPath(Type type)
        {
            return ReferenceAssemblyResolvers[OSInfo.Current].ResolveReferenceAssemblyPath(type);
        }
    }
}