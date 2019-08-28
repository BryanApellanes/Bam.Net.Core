using System;

namespace Bam.Net.Data
{
    public class QueryFilterValue
    {
        public QueryFilterValue(object value)
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

        public object GetValue()
        {
            return Type == typeof(ulong) ? Dao.MapUlongToLong((ulong)Value) : Value;
        }

        public bool IsNull()
        {
            return Value == null || Type == typeof(DBNull) || Value?.GetType() == typeof(DBNull);
        }
    }
}