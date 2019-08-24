/*
	This file was generated and should not be modified directly
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.Services.DataReplication.Data.Dao
{
    public class DataPointOriginQuery: Query<DataPointOriginColumns, DataPointOrigin>
    { 
		public DataPointOriginQuery(){}
		public DataPointOriginQuery(WhereDelegate<DataPointOriginColumns> where, OrderBy<DataPointOriginColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }
		public DataPointOriginQuery(Func<DataPointOriginColumns, QueryFilter<DataPointOriginColumns>> where, OrderBy<DataPointOriginColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }		
		public DataPointOriginQuery(Delegate where, Database db = null) : base(where, db) { }
		
        public static DataPointOriginQuery Where(WhereDelegate<DataPointOriginColumns> where)
        {
            return Where(where, null, null);
        }

        public static DataPointOriginQuery Where(WhereDelegate<DataPointOriginColumns> where, OrderBy<DataPointOriginColumns> orderBy = null, Database db = null)
        {
            return new DataPointOriginQuery(where, orderBy, db);
        }

		public DataPointOriginCollection Execute()
		{
			return new DataPointOriginCollection(this, true);
		}
    }
}