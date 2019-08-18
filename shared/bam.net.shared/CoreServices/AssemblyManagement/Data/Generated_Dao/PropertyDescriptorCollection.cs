using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.CoreServices.AssemblyManagement.Data.Dao
{
    public class PropertyDescriptorCollection: DaoCollection<PropertyDescriptorColumns, PropertyDescriptor>
    { 
		public PropertyDescriptorCollection(){}
		public PropertyDescriptorCollection(Database db, DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public PropertyDescriptorCollection(DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public PropertyDescriptorCollection(Query<PropertyDescriptorColumns, PropertyDescriptor> q, Bam.Net.Data.Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public PropertyDescriptorCollection(Database db, Query<PropertyDescriptorColumns, PropertyDescriptor> q, bool load) : base(db, q, load) { }
		public PropertyDescriptorCollection(Query<PropertyDescriptorColumns, PropertyDescriptor> q, bool load) : base(q, load) { }
    }
}