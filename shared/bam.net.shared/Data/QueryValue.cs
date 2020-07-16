using System;

namespace Bam.Net.Data
{
    /// <summary>
    /// Represents a value used in a QueryFilter.
    /// </summary>
    public class QueryValue
    {
        public QueryValue(object value, QueryFilter filter = null)
        {
            QueryFilter = filter ?? new QueryFilter();
            if (value == null)
            {
                Type = typeof(DBNull);
            }
            else
            {
                Type = value.GetType();
                Value = value;
            }
        }

        public QueryFilter QueryFilter { get; set; }
        public Type Type { get; set; }
        public object Value { get; private set; }

        /// <summary>
        /// If the value is a ulong then the stored value is mapped to a long to account for some storage engines.
        /// The mapped long is returned.
        /// </summary>
        /// <returns></returns>
        public virtual object GetStoredValue()
        {
            return GetValue(true);
        }
        
        public virtual object GetRawValue()
        {
            return Value;
        }

        public virtual object GetValue()
        {
            if (QueryFilter.Property<bool>("IsForeignKey", false))
            {
                return GetRawValue();
            }
            return GetStoredValue();
        }
        
        public virtual object GetValue(bool mapUlongToLong)
        {
            return (Type == typeof(ulong) && mapUlongToLong) ? Dao.MapUlongToLong((ulong)Value) : Value;
        }

        public bool IsNull()
        {
            return Value == null || Type == typeof(DBNull) || Value?.GetType() == typeof(DBNull);
        }
    }
}