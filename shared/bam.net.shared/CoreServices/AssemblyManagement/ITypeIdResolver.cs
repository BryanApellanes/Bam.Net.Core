using System;
using System.Reflection;

namespace Bam.Net.CoreServices
{
    public interface ITypeIdResolver: ITypeResolver
    {
        long ResolveTypeId(string typeNamespace, string typeName);
        long ResolveTypeId(string namespaceQualifiedTypeName);
        long ResolveTypeId(Type type);
        long ResolvePropertyId(PropertyInfo prop);
        long ResolvePropertyId(PropertyInfo prop, out string name);
        long ResolvePropertyId(string typeNamespace, string typeName, string propertyName);
        long ResolvePropertyId(string namespaceQualifiedTypePropertyName);
    }
}