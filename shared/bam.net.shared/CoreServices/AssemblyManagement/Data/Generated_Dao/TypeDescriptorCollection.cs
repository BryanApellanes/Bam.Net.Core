using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.CoreServices.AssemblyManagement.Data.Dao
{
    public class TypeDescriptorCollection: DaoCollection<TypeDescriptorColumns, TypeDescriptor>
    { 
		public TypeDescriptorCollection(){}
		public TypeDescriptorCollection(Database db, DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public TypeDescriptorCollection(DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public TypeDescriptorCollection(Query<TypeDescriptorColumns, TypeDescriptor> q, Bam.Net.Data.Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public TypeDescriptorCollection(Database db, Query<TypeDescriptorColumns, TypeDescriptor> q, bool load) : base(db, q, load) { }
		public TypeDescriptorCollection(Query<TypeDescriptorColumns, TypeDescriptor> q, bool load) : base(q, load) { }
    }
}