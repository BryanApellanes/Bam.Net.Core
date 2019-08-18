using System;
using System.Collections.Generic;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.CoreServices.AssemblyManagement.Data.Dao
{
    public class PropertyDescriptorColumns: QueryFilter<PropertyDescriptorColumns>, IFilterToken
    {
        public PropertyDescriptorColumns() { }
        public PropertyDescriptorColumns(string columnName)
            : base(columnName)
        { }
		
		public PropertyDescriptorColumns KeyColumn
		{
			get
			{
				return new PropertyDescriptorColumns("Id");
			}
		}	

        public PropertyDescriptorColumns Id
        {
            get
            {
                return new PropertyDescriptorColumns("Id");
            }
        }
        public PropertyDescriptorColumns Uuid
        {
            get
            {
                return new PropertyDescriptorColumns("Uuid");
            }
        }
        public PropertyDescriptorColumns Cuid
        {
            get
            {
                return new PropertyDescriptorColumns("Cuid");
            }
        }
        public PropertyDescriptorColumns ParentTypeDescriptorKey
        {
            get
            {
                return new PropertyDescriptorColumns("ParentTypeDescriptorKey");
            }
        }
        public PropertyDescriptorColumns PropertyName
        {
            get
            {
                return new PropertyDescriptorColumns("PropertyName");
            }
        }
        public PropertyDescriptorColumns CompositeKeyId
        {
            get
            {
                return new PropertyDescriptorColumns("CompositeKeyId");
            }
        }
        public PropertyDescriptorColumns CompositeKey
        {
            get
            {
                return new PropertyDescriptorColumns("CompositeKey");
            }
        }
        public PropertyDescriptorColumns CreatedBy
        {
            get
            {
                return new PropertyDescriptorColumns("CreatedBy");
            }
        }
        public PropertyDescriptorColumns ModifiedBy
        {
            get
            {
                return new PropertyDescriptorColumns("ModifiedBy");
            }
        }
        public PropertyDescriptorColumns Modified
        {
            get
            {
                return new PropertyDescriptorColumns("Modified");
            }
        }
        public PropertyDescriptorColumns Deleted
        {
            get
            {
                return new PropertyDescriptorColumns("Deleted");
            }
        }
        public PropertyDescriptorColumns Created
        {
            get
            {
                return new PropertyDescriptorColumns("Created");
            }
        }



		protected internal Type TableType
		{
			get
			{
				return typeof(PropertyDescriptor);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}