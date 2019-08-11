using System;
using Bam.Net.Data;
using Bam.Net.Data.Repositories;

namespace Bam.Net.Services.DataReplication.Data
{
    public class DataPropertyFilter : DataProperty
    {
        public QueryOperator Operator { get; set; }
        public virtual ulong QueryOperationId { get; set; }
        public virtual QueryOperation QueryOperation { get; set; }

        public QueryFilter ToQueryFilter()
        {
            QueryFilter filter = new QueryFilter(Name);
            switch (Operator)
            {
                case QueryOperator.NotEqualTo:
                    filter = filter != ValueConverter(Value);
                    break;
                case QueryOperator.GreaterThan:
                    filter = filter > ValueConverter(Value);
                    break;
                case QueryOperator.LessThan:
                    filter = filter < ValueConverter(Value);
                    break;
                case QueryOperator.StartsWith:
                    filter = filter.StartsWith(ValueConverter(Value));
                    break;
                case QueryOperator.DoesntStartWith:
                    filter = filter.DoesntStartWith(ValueConverter(Value));
                    break;
                case QueryOperator.Contains:
                    filter = filter.Contains(ValueConverter(Value));
                    break;
                case QueryOperator.DoesntContain:
                    filter = filter.DoesntContain(ValueConverter(Value));
                    break;
                case QueryOperator.In:
                    filter = filter.In(ArrayConverter(Value));
                    break;
                case QueryOperator.Invalid:
                case QueryOperator.Equals:
                default:
                    filter = filter == ValueConverter(Value);
                    break;
            }

            return filter;
        }

        public static DataPropertyFilter Where(object property, QueryOperator queryOperator, object value)
        {
            return DataProperty.FilterWhere(property, queryOperator, value);
        }
    }
}