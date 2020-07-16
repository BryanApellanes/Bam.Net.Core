using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.Data.Repositories
{
    internal static class CompositeKeyHashProvider
    {
        public static ulong GetDeterministicId(object instance)
        {
            Args.ThrowIfNull(instance, "instance");
            string[] compositeKeyProperties = GetCompositeKeyProperties(instance.GetType());
            return GetULongKeyHash(instance, instance.Property("PropertyDelimiter")?.ToString().Or("\r\n"), compositeKeyProperties);
        }
        
        public static string[] GetCompositeKeyProperties(Type type)
        {
            List<string> props = type.GetPropertiesWithAttributeOfType<CompositeKeyAttribute>().Select(pi => pi.Name).ToList();
            Args.ThrowIf(props.Count == 0, "No properties adorned with CompositeKeyAttribute defined on type ({0})", type.Name);
            props.Sort();
            return props.ToArray();
        }

        public static string GetStringKeyHash(object instance, HashAlgorithms algorithm = HashAlgorithms.SHA256)
        {
            Args.ThrowIfNull(instance);
            return GetStringKeyHash(instance, "\r\n", GetCompositeKeyProperties(instance.GetType()), algorithm);
        }

        internal static string GetStringKeyHash(object instance, string propertyDelimiter, HashAlgorithms algorithm)
        {
            Args.ThrowIfNull(instance);
            return GetStringKeyHash(instance, propertyDelimiter, GetCompositeKeyProperties(instance.GetType()),
                algorithm);
        }
        
        internal static string GetStringKeyHash(object instance, string propertyDelimiter,
            string[] compositeKeyProperties, HashAlgorithms algorithm)
        {
            CheckArgs(instance, compositeKeyProperties);
            return string.Join(propertyDelimiter, compositeKeyProperties.Select(prop => instance.Property(prop)))
                .HashHexString(algorithm);
        }

        internal static int GetIntKeyHash(object instance, string propertyDelimiter, string[] compositeKeyProperties)
        {
            CheckArgs(instance, compositeKeyProperties);
            return string.Join(propertyDelimiter, compositeKeyProperties.Select(prop => instance.Property(prop))).ToSha256Int();
        }

        internal static long GetLongKeyHash(object instance, string propertyDelimiter, string[] compositeKeyProperties)
        {
            CheckArgs(instance, compositeKeyProperties);
            return string.Join(propertyDelimiter, compositeKeyProperties.Select(prop => instance.Property(prop))).ToSha256Long();
        }

        internal static ulong GetULongKeyHash(object instance, string propertyDelimiter, string[] compositeKeyProperties)
        {
            CheckArgs(instance, compositeKeyProperties);
            return string.Join(propertyDelimiter, compositeKeyProperties.Select(prop => instance.Property(prop))).ToSha256ULong();
        }
        
        private static void CheckArgs(object instance, string[] compositeKeyProperties)
        {
            Args.ThrowIfNull(instance);
            Args.ThrowIfNull(compositeKeyProperties);
            Args.ThrowIf(compositeKeyProperties.Length == 0,
                $"No CompositeKeyProperties specified for type ({instance.GetType().Name})");
        }
    }
}
