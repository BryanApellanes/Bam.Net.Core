using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.CoreServices.ApplicationRegistration.Data.Dao
{
    public class ApplicationHostDomainCollection: DaoCollection<ApplicationHostDomainColumns, ApplicationHostDomain>
    { 
		public ApplicationHostDomainCollection(){}
		public ApplicationHostDomainCollection(Database db, DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public ApplicationHostDomainCollection(DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public ApplicationHostDomainCollection(Query<ApplicationHostDomainColumns, ApplicationHostDomain> q, Bam.Net.Data.Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public ApplicationHostDomainCollection(Database db, Query<ApplicationHostDomainColumns, ApplicationHostDomain> q, bool load) : base(db, q, load) { }
		public ApplicationHostDomainCollection(Query<ApplicationHostDomainColumns, ApplicationHostDomain> q, bool load) : base(q, load) { }
    }
}