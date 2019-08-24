using System;
using System.Collections.Generic;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.CoreServices.AssemblyManagement.Data.Dao
{
    public class TypeDescriptorColumns: QueryFilter<TypeDescriptorColumns>, IFilterToken
    {
        public TypeDescriptorColumns() { }
        public TypeDescriptorColumns(string columnName)
            : base(columnName)
        { }
		
		public TypeDescriptorColumns KeyColumn
		{
			get
			{
				return new TypeDescriptorColumns("Id");
			}
		}	

        public TypeDescriptorColumns Id
        {
            get
            {
                return new TypeDescriptorColumns("Id");
            }
        }
        public TypeDescriptorColumns Uuid
        {
            get
            {
                return new TypeDescriptorColumns("Uuid");
            }
        }
        public TypeDescriptorColumns Cuid
        {
            get
            {
                return new TypeDescriptorColumns("Cuid");
            }
        }
        public TypeDescriptorColumns Namespace
        {
            get
            {
                return new TypeDescriptorColumns("Namespace");
            }
        }
        public TypeDescriptorColumns TypeName
        {
            get
            {
                return new TypeDescriptorColumns("TypeName");
            }
        }
        public TypeDescriptorColumns CompositeKeyId
        {
            get
            {
                return new TypeDescriptorColumns("CompositeKeyId");
            }
        }
        public TypeDescriptorColumns CompositeKey
        {
            get
            {
                return new TypeDescriptorColumns("CompositeKey");
            }
        }
        public TypeDescriptorColumns CreatedBy
        {
            get
            {
                return new TypeDescriptorColumns("CreatedBy");
            }
        }
        public TypeDescriptorColumns ModifiedBy
        {
            get
            {
                return new TypeDescriptorColumns("ModifiedBy");
            }
        }
        public TypeDescriptorColumns Modified
        {
            get
            {
                return new TypeDescriptorColumns("Modified");
            }
        }
        public TypeDescriptorColumns Deleted
        {
            get
            {
                return new TypeDescriptorColumns("Deleted");
            }
        }
        public TypeDescriptorColumns Created
        {
            get
            {
                return new TypeDescriptorColumns("Created");
            }
        }



		protected internal Type TableType
		{
			get
			{
				return typeof(TypeDescriptor);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}