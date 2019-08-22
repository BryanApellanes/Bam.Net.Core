using System;
using System.Collections.Generic;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.CoreServices.ApplicationRegistration.Data.Dao
{
    public class ApplicationHostDomainColumns: QueryFilter<ApplicationHostDomainColumns>, IFilterToken
    {
        public ApplicationHostDomainColumns() { }
        public ApplicationHostDomainColumns(string columnName)
            : base(columnName)
        { }
		
		public ApplicationHostDomainColumns KeyColumn
		{
			get
			{
				return new ApplicationHostDomainColumns("Id");
			}
		}	

        public ApplicationHostDomainColumns Id
        {
            get
            {
                return new ApplicationHostDomainColumns("Id");
            }
        }
        public ApplicationHostDomainColumns Uuid
        {
            get
            {
                return new ApplicationHostDomainColumns("Uuid");
            }
        }


        public ApplicationHostDomainColumns ApplicationId
        {
            get
            {
                return new ApplicationHostDomainColumns("ApplicationId");
            }
        }
        public ApplicationHostDomainColumns HostDomainId
        {
            get
            {
                return new ApplicationHostDomainColumns("HostDomainId");
            }
        }

		protected internal Type TableType
		{
			get
			{
				return typeof(ApplicationHostDomain);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}