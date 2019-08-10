using System;
using Bam.Net.Data;
using Bam.Net.Data.Repositories;

namespace Bam.Net.Services.DataReplication.Data
{
    public class DataPropertyFilter : DataProperty
    {
        public QueryOperator Operator { get; set; }
        public virtual ulong QueryOperationId { get; set; }

        public QueryFilter ToQueryFilter()
        {
            QueryFilter filter = new QueryFilter(Name);
            switch (Operator)
            {
                    /*Invalid,
                    Equals,
                    NotEqualTo,
                    GreaterThan,
                    LessThan,
                    StartsWith,
                    DoesntStartWith,
                    EndsWith,
                    DoesntEndWith,
                    Contains,
                    DoesntContain,
                    OpenParen,
                    CloseParen,
                    AND,
                    OR*/
            }

            throw new NotImplementedException();
            
            return filter;
        }
        
    }
}