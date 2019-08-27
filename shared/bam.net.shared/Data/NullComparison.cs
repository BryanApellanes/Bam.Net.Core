/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bam.Net.Data
{
    public class NullComparison: Comparison
    {
        public NullComparison(string columnName, string oper)
            : base(columnName, oper, null)
        { }

        public override string ToString()
        {
            return $"{ColumnName} {this.Operator} NULL";
        }
    }
}
