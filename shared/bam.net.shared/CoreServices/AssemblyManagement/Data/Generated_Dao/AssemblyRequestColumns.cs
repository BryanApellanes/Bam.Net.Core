using System;
using System.Collections.Generic;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.CoreServices.AssemblyManagement.Data.Dao
{
    public class AssemblyRequestColumns: QueryFilter<AssemblyRequestColumns>, IFilterToken
    {
        public AssemblyRequestColumns() { }
        public AssemblyRequestColumns(string columnName)
            : base(columnName)
        { }
		
		public AssemblyRequestColumns KeyColumn
		{
			get
			{
				return new AssemblyRequestColumns("Id");
			}
		}	

        public AssemblyRequestColumns Id
        {
            get
            {
                return new AssemblyRequestColumns("Id");
            }
        }
        public AssemblyRequestColumns Uuid
        {
            get
            {
                return new AssemblyRequestColumns("Uuid");
            }
        }
        public AssemblyRequestColumns Cuid
        {
            get
            {
                return new AssemblyRequestColumns("Cuid");
            }
        }
        public AssemblyRequestColumns FileHash
        {
            get
            {
                return new AssemblyRequestColumns("FileHash");
            }
        }
        public AssemblyRequestColumns AssemblyFullName
        {
            get
            {
                return new AssemblyRequestColumns("AssemblyFullName");
            }
        }
        public AssemblyRequestColumns Name
        {
            get
            {
                return new AssemblyRequestColumns("Name");
            }
        }
        public AssemblyRequestColumns RequestingAssemblyFullName
        {
            get
            {
                return new AssemblyRequestColumns("RequestingAssemblyFullName");
            }
        }
        public AssemblyRequestColumns Created
        {
            get
            {
                return new AssemblyRequestColumns("Created");
            }
        }



		protected internal Type TableType
		{
			get
			{
				return typeof(AssemblyRequest);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}