using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Data.Common;
using System.Data;
using System.Collections;
using Bam.Net.Logging;
using Newtonsoft.Json.Linq;
using NLog.LayoutRenderers;

namespace Bam.Net.Data
{
    public static partial class DataExtensions
    {
        public static object _toJsonSafeLock = new object();
        public static readonly Dictionary<object, JObject> _visited = new Dictionary<object, JObject>();

        public static object ToJsonSafe(this object obj)
        {
            return ToJsonSafe(obj, 5);
        }
        
        /// <summary>
        /// Create a json safe version of the object
        /// by creating a dynamic type that represents
        /// the properties on the original object
        /// that are adorned with the ColumnAttribute
        /// custom attribute.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object ToJsonSafe(this object obj, int maxRecursion)
        {
            return ToJsonSafe(obj, maxRecursion, 0);
        }
        
        private static object ToJsonSafe(this object obj, int maxRecursion, int recursionThusFar)
        {
            Args.ThrowIfNull(obj, "obj");
            if (_visited.ContainsKey(obj))
            {
                return _visited[obj];
            }

            if (recursionThusFar >= maxRecursion)
            {
                Log.Warn("{0}: Max recursion reached ({1}) for instance of type ({2})", nameof(ToJsonSafe), maxRecursion, obj.GetType().Name);
                return null;
            }
            JObject jobj = new JObject();
            Type type = obj.GetType();
            IEnumerable<PropertyInfo> properties = type.GetProperties().Where(pi => pi.HasCustomAttributeOfType<ColumnAttribute>());
            foreach (PropertyInfo prop in properties)
            {
                object val = prop.GetValue(obj);
                if (val != null)
                {
                    if (prop.PropertyType.IsPrimitiveNullableOrString() || prop.PropertyType.IsNullable<DateTime>())
                    {
                        jobj.Add(prop.Name, new JValue(val));
                    }
                    else
                    {
                        _visited.Add(val, (JObject)ToJsonSafe(val, maxRecursion, ++recursionThusFar));
                        jobj.Add(prop.Name, _visited[val]);                        
                    }
                }
                else
                {
                    jobj.Add(prop.Name, null);
                }
            }
            return jobj;
        }
    }
}
