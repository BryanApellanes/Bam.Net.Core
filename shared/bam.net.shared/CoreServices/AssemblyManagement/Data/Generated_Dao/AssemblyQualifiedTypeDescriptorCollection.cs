using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.CoreServices.AssemblyManagement.Data.Dao
{
    public class AssemblyQualifiedTypeDescriptorCollection: DaoCollection<AssemblyQualifiedTypeDescriptorColumns, AssemblyQualifiedTypeDescriptor>
    { 
		public AssemblyQualifiedTypeDescriptorCollection(){}
		public AssemblyQualifiedTypeDescriptorCollection(Database db, DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public AssemblyQualifiedTypeDescriptorCollection(DataTable table, Bam.Net.Data.Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public AssemblyQualifiedTypeDescriptorCollection(Query<AssemblyQualifiedTypeDescriptorColumns, AssemblyQualifiedTypeDescriptor> q, Bam.Net.Data.Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public AssemblyQualifiedTypeDescriptorCollection(Database db, Query<AssemblyQualifiedTypeDescriptorColumns, AssemblyQualifiedTypeDescriptor> q, bool load) : base(db, q, load) { }
		public AssemblyQualifiedTypeDescriptorCollection(Query<AssemblyQualifiedTypeDescriptorColumns, AssemblyQualifiedTypeDescriptor> q, bool load) : base(q, load) { }
    }
}