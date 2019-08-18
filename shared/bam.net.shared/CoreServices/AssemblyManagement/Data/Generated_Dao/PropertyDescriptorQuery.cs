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
    public class PropertyDescriptorQuery: Query<PropertyDescriptorColumns, PropertyDescriptor>
    { 
		public PropertyDescriptorQuery(){}
		public PropertyDescriptorQuery(WhereDelegate<PropertyDescriptorColumns> where, OrderBy<PropertyDescriptorColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }
		public PropertyDescriptorQuery(Func<PropertyDescriptorColumns, QueryFilter<PropertyDescriptorColumns>> where, OrderBy<PropertyDescriptorColumns> orderBy = null, Database db = null) : base(where, orderBy, db) { }		
		public PropertyDescriptorQuery(Delegate where, Database db = null) : base(where, db) { }
		
        public static PropertyDescriptorQuery Where(WhereDelegate<PropertyDescriptorColumns> where)
        {
            return Where(where, null, null);
        }

        public static PropertyDescriptorQuery Where(WhereDelegate<PropertyDescriptorColumns> where, OrderBy<PropertyDescriptorColumns> orderBy = null, Database db = null)
        {
            return new PropertyDescriptorQuery(where, orderBy, db);
        }

		public PropertyDescriptorCollection Execute()
		{
			return new PropertyDescriptorCollection(this, true);
		}
    }
}