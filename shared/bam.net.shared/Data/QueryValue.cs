using System;

namespace Bam.Net.Data
{
    /// <summary>
    /// Represents a value used in a QueryFilter.
    /// </summary>
    public class QueryValue
    {
        public QueryValue(object value)
        {
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
        
        public Type Type { get; set; }
        public object Value { get; set; }

        public virtual object GetValue()
        {
            return GetValue(true);
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