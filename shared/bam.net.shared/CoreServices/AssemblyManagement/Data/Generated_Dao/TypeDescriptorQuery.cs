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
    public class TypeDescriptorQuery: Query<TypeDescriptorColumns, TypeDescriptor>
    { 
		public TypeDescriptorQuery(){}
		public TypeDescriptorQuery(WhereDelegate<TypeDescriptorColumns> where, OrderBy<TypeDescriptorColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }
		public TypeDescriptorQuery(Func<TypeDescriptorColumns, QueryFilter<TypeDescriptorColumns>> where, OrderBy<TypeDescriptorColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }		
		public TypeDescriptorQuery(Delegate where, Database db = null) : base(where, db) { }
		
        public static TypeDescriptorQuery Where(WhereDelegate<TypeDescriptorColumns> where)
        {
            return Where(where, null, null);
        }

        public static TypeDescriptorQuery Where(WhereDelegate<TypeDescriptorColumns> where, OrderBy<TypeDescriptorColumns> orderBy = null, Database db = null)
        {
            return new TypeDescriptorQuery(where, orderBy, db);
        }

		public TypeDescriptorCollection Execute()
		{
			return new TypeDescriptorCollection(this, true);
		}
    }
}