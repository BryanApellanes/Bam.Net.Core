/*
	This file was generated and should not be modified directly
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.CoreServices.AssemblyManagement.Data.Dao
{
    public class AssemblyRequestQuery: Query<AssemblyRequestColumns, AssemblyRequest>
    { 
		public AssemblyRequestQuery(){}
		public AssemblyRequestQuery(WhereDelegate<AssemblyRequestColumns> where, OrderBy<AssemblyRequestColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }
		public AssemblyRequestQuery(Func<AssemblyRequestColumns, QueryFilter<AssemblyRequestColumns>> where, OrderBy<AssemblyRequestColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }		
		public AssemblyRequestQuery(Delegate where, Database db = null) : base(where, db) { }
		
        public static AssemblyRequestQuery Where(WhereDelegate<AssemblyRequestColumns> where)
        {
            return Where(where, null, null);
        }

        public static AssemblyRequestQuery Where(WhereDelegate<AssemblyRequestColumns> where, OrderBy<AssemblyRequestColumns> orderBy = null, Database db = null)
        {
            return new AssemblyRequestQuery(where, orderBy, db);
        }

		public AssemblyRequestCollection Execute()
		{
			return new AssemblyRequestCollection(this, true);
		}
    }
}