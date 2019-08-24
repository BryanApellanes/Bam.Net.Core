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
    public class AssemblyQualifiedTypeDescriptorQuery: Query<AssemblyQualifiedTypeDescriptorColumns, AssemblyQualifiedTypeDescriptor>
    { 
		public AssemblyQualifiedTypeDescriptorQuery(){}
		public AssemblyQualifiedTypeDescriptorQuery(WhereDelegate<AssemblyQualifiedTypeDescriptorColumns> where, OrderBy<AssemblyQualifiedTypeDescriptorColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }
		public AssemblyQualifiedTypeDescriptorQuery(Func<AssemblyQualifiedTypeDescriptorColumns, QueryFilter<AssemblyQualifiedTypeDescriptorColumns>> where, OrderBy<AssemblyQualifiedTypeDescriptorColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }		
		public AssemblyQualifiedTypeDescriptorQuery(Delegate where, Database db = null) : base(where, db) { }
		
        public static AssemblyQualifiedTypeDescriptorQuery Where(WhereDelegate<AssemblyQualifiedTypeDescriptorColumns> where)
        {
            return Where(where, null, null);
        }

        public static AssemblyQualifiedTypeDescriptorQuery Where(WhereDelegate<AssemblyQualifiedTypeDescriptorColumns> where, OrderBy<AssemblyQualifiedTypeDescriptorColumns> orderBy = null, Database db = null)
        {
            return new AssemblyQualifiedTypeDescriptorQuery(where, orderBy, db);
        }

		public AssemblyQualifiedTypeDescriptorCollection Execute()
		{
			return new AssemblyQualifiedTypeDescriptorCollection(this, true);
		}
    }
}