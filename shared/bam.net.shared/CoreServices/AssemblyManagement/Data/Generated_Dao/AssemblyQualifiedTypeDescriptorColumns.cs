using System;
using System.Collections.Generic;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.CoreServices.AssemblyManagement.Data.Dao
{
    public class AssemblyQualifiedTypeDescriptorColumns: QueryFilter<AssemblyQualifiedTypeDescriptorColumns>, IFilterToken
    {
        public AssemblyQualifiedTypeDescriptorColumns() { }
        public AssemblyQualifiedTypeDescriptorColumns(string columnName)
            : base(columnName)
        { }
		
		public AssemblyQualifiedTypeDescriptorColumns KeyColumn
		{
			get
			{
				return new AssemblyQualifiedTypeDescriptorColumns("Id");
			}
		}	

        public AssemblyQualifiedTypeDescriptorColumns Id
        {
            get
            {
                return new AssemblyQualifiedTypeDescriptorColumns("Id");
            }
        }
        public AssemblyQualifiedTypeDescriptorColumns Uuid
        {
            get
            {
                return new AssemblyQualifiedTypeDescriptorColumns("Uuid");
            }
        }
        public AssemblyQualifiedTypeDescriptorColumns Cuid
        {
            get
            {
                return new AssemblyQualifiedTypeDescriptorColumns("Cuid");
            }
        }
        public AssemblyQualifiedTypeDescriptorColumns AssemblyPath
        {
            get
            {
                return new AssemblyQualifiedTypeDescriptorColumns("AssemblyPath");
            }
        }
        public AssemblyQualifiedTypeDescriptorColumns Namespace
        {
            get
            {
                return new AssemblyQualifiedTypeDescriptorColumns("Namespace");
            }
        }
        public AssemblyQualifiedTypeDescriptorColumns TypeName
        {
            get
            {
                return new AssemblyQualifiedTypeDescriptorColumns("TypeName");
            }
        }
        public AssemblyQualifiedTypeDescriptorColumns CompositeKeyId
        {
            get
            {
                return new AssemblyQualifiedTypeDescriptorColumns("CompositeKeyId");
            }
        }
        public AssemblyQualifiedTypeDescriptorColumns CompositeKey
        {
            get
            {
                return new AssemblyQualifiedTypeDescriptorColumns("CompositeKey");
            }
        }
        public AssemblyQualifiedTypeDescriptorColumns CreatedBy
        {
            get
            {
                return new AssemblyQualifiedTypeDescriptorColumns("CreatedBy");
            }
        }
        public AssemblyQualifiedTypeDescriptorColumns ModifiedBy
        {
            get
            {
                return new AssemblyQualifiedTypeDescriptorColumns("ModifiedBy");
            }
        }
        public AssemblyQualifiedTypeDescriptorColumns Modified
        {
            get
            {
                return new AssemblyQualifiedTypeDescriptorColumns("Modified");
            }
        }
        public AssemblyQualifiedTypeDescriptorColumns Deleted
        {
            get
            {
                return new AssemblyQualifiedTypeDescriptorColumns("Deleted");
            }
        }
        public AssemblyQualifiedTypeDescriptorColumns Created
        {
            get
            {
                return new AssemblyQualifiedTypeDescriptorColumns("Created");
            }
        }



		protected internal Type TableType
		{
			get
			{
				return typeof(AssemblyQualifiedTypeDescriptor);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}