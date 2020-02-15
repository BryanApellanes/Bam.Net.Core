using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net
{
    public class TypeResolver : ITypeResolver
    {
        public TypeResolver()
        {
            _assemblies = new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies());
        }

        static TypeResolver _defaultTypeResolver;
        static readonly object _defaultLock = new object();
        public static TypeResolver Default
        {
            get { return _defaultLock.DoubleCheckLock(ref _defaultTypeResolver, () => new TypeResolver()); }
        }

        readonly List<Assembly> _assemblies;
        public Assembly[] Assemblies => _assemblies.ToArray();

        public bool ScanAssemblies { get; set; }

        public void AddAssembly(string assemblyFilePath)
        {
            Assembly assembly = Assembly.LoadFrom(assemblyFilePath);
            if(assembly != null)
            {
                _assemblies.Add(assembly);
            }
        }

        public virtual Type ResolveType(string typeName)
        {
            Type type = Type.GetType(typeName);
            if(type == null && ScanAssemblies)
            {
                foreach(Assembly a in Assemblies)
                {
                    type = a.GetType(typeName);
                    if(type == null)
                    {
                        type = a.GetTypes().FirstOrDefault(t => t.Name.Equals(typeName) || $"{t.Namespace}.{t.Name}".Equals(typeName));
                    }

                    if(type != null)
                    {
                        break;
                    }
                }
            }
            return type;
        }

        public virtual Type ResolveType(string nameSpace, string typeName)
        {
            return ResolveType($"{nameSpace}.{typeName}");
        }
    }
}
