/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.Data.Npgsql
{
    /// <summary>
    /// Provides Npgsql specific expression formatting.
    /// It may make sense to put this class into the database.ServiceProvider
    /// container, especially when moving on to implement 
    /// support for other databases.  
    /// </summary>
    internal class NpgsqlFormatProvider
    {
        static NpgsqlFormatProvider()
        {
            ColumnNameFormatter = (s) => $"\"{s}\"";
        }
        
        public static Func<string, string> ColumnNameFormatter
        {
            get;
            set;
        }
        
        public static SetFormat GetSetFormat(string tableName, StringBuilder stringBuilder, int? startNumber, params AssignValue[] values)
        {
            SetFormat set = new SetFormat();
            set.ColumnNameFormatter = ColumnNameFormatter;
            set.ParameterPrefix = ":";
            foreach (AssignValue value in values)
            {
                value.ColumnNameFormatter = set.ColumnNameFormatter;
                set.AddAssignment(value);
            }

            set.StartNumber = startNumber;
            stringBuilder.Append(set.Parse());
            return set;
        }

        public static WhereFormat GetWhereFormat(IQueryFilter filter, StringBuilder stringBuilder, int? startNumber)
        {
            WhereFormat where = new WhereFormat(filter);
            where.ColumnNameFormatter = ColumnNameFormatter;
            where.ParameterPrefix = ":";
            where.StartNumber = startNumber;
            stringBuilder.Append(where.Parse());
            return where;
        }

        public static WhereFormat GetWhereFormat(AssignValue filter, StringBuilder stringBuilder, int? startNumber)
        {
            WhereFormat where = new WhereFormat();
            where.ColumnNameFormatter = ColumnNameFormatter;
            where.ParameterPrefix = ":";
            where.StartNumber = startNumber;
            where.AddAssignment(filter);
            stringBuilder.Append(where.Parse());
            return where;
        }

        public static AndFormat GetAndFormat(AssignValue filter, StringBuilder stringBuilder, int? startNumber)
        {
            AndFormat and = new AndFormat();
            and.ColumnNameFormatter = ColumnNameFormatter;
            and.ParameterPrefix = ":";
            and.StartNumber = startNumber;
            and.AddAssignment(filter);
            stringBuilder.Append(and.Parse());
            return and;
        }
        
        public static AndFormat GetAndFormat(IQueryFilter filter, StringBuilder stringBuilder, int? startNumber)
        {
            AndFormat and = new AndFormat(filter);
            and.ColumnNameFormatter = ColumnNameFormatter;
            and.ParameterPrefix = ":";
            and.StartNumber = startNumber;
            stringBuilder.Append(and.Parse());
            return and;
        }
    }
}
