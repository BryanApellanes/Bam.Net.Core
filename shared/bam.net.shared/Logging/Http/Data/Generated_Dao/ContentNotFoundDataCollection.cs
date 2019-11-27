using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.Logging.Http.Data.Dao
{
    public class ContentNotFoundDataCollection: DaoCollection<ContentNotFoundDataColumns, ContentNotFoundData>
    { 
		public ContentNotFoundDataCollection(){}
		public ContentNotFoundDataCollection(Database db, DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public ContentNotFoundDataCollection(DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public ContentNotFoundDataCollection(Query<ContentNotFoundDataColumns, ContentNotFoundData> q, Bam.Net.Data.Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public ContentNotFoundDataCollection(Database db, Query<ContentNotFoundDataColumns, ContentNotFoundData> q, bool load) : base(db, q, load) { }
		public ContentNotFoundDataCollection(Query<ContentNotFoundDataColumns, ContentNotFoundData> q, bool load) : base(q, load) { }
    }
}