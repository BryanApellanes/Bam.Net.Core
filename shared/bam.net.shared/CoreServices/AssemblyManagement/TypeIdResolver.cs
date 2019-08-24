using System;
using System.Reflection;
using Bam.Net.CoreServices;
using Bam.Net.Services.DataReplication;

namespace Bam.Net
{
    /// <summary>
    /// A TypeResolver that uses local TypeMap to resolve types and type ids.
    /// </summary>
    public class TypeIdResolver : TypeResolver, ITypeIdResolver
    {
        public long ResolveTypeId(string typeNamespace, string typeName)
        {
            throw new NotImplementedException();
        }

        public long ResolveTypeId(string namespaceQualifiedTypeName)
        {
            throw new NotImplementedException();
        }

        public long ResolveTypeId(Type type)
        {
            throw new NotImplementedException();
        }

        public long ResolvePropertyId(PropertyInfo prop)
        {
            throw new NotImplementedException();
        }

        public long ResolvePropertyId(PropertyInfo prop, out string name)
        {
            throw new NotImplementedException();
        }

        public long ResolvePropertyId(string typeNamespace, string typeName, string propertyName)
        {
            throw new NotImplementedException();
        }

        public long ResolvePropertyId(string namespaceQualifiedTypePropertyName)
        {
            throw new NotImplementedException();
        }
    }
}