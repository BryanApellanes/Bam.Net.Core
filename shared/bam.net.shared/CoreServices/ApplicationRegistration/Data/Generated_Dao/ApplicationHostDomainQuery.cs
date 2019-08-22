/*
	This file was generated and should not be modified directly
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.CoreServices.ApplicationRegistration.Data.Dao
{
    public class ApplicationHostDomainQuery: Query<ApplicationHostDomainColumns, ApplicationHostDomain>
    { 
		public ApplicationHostDomainQuery(){}
		public ApplicationHostDomainQuery(WhereDelegate<ApplicationHostDomainColumns> where, OrderBy<ApplicationHostDomainColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }
		public ApplicationHostDomainQuery(Func<ApplicationHostDomainColumns, QueryFilter<ApplicationHostDomainColumns>> where, OrderBy<ApplicationHostDomainColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }		
		public ApplicationHostDomainQuery(Delegate where, Database db = null) : base(where, db) { }
		
        public static ApplicationHostDomainQuery Where(WhereDelegate<ApplicationHostDomainColumns> where)
        {
            return Where(where, null, null);
        }

        public static ApplicationHostDomainQuery Where(WhereDelegate<ApplicationHostDomainColumns> where, OrderBy<ApplicationHostDomainColumns> orderBy = null, Database db = null)
        {
            return new ApplicationHostDomainQuery(where, orderBy, db);
        }

		public ApplicationHostDomainCollection Execute()
		{
			return new ApplicationHostDomainCollection(this, true);
		}
    }
}