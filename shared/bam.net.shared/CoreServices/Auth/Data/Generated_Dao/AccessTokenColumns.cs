using System;
using System.Collections.Generic;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.CoreServices.Auth.Data.Dao
{
    public class AccessTokenColumns: QueryFilter<AccessTokenColumns>, IFilterToken
    {
        public AccessTokenColumns() { }
        public AccessTokenColumns(string columnName)
            : base(columnName)
        { }
		
		public AccessTokenColumns KeyColumn
		{
			get
			{
				return new AccessTokenColumns("Id");
			}
		}	

        public AccessTokenColumns Id
        {
            get
            {
                return new AccessTokenColumns("Id");
            }
        }
        public AccessTokenColumns Uuid
        {
            get
            {
                return new AccessTokenColumns("Uuid");
            }
        }
        public AccessTokenColumns Cuid
        {
            get
            {
                return new AccessTokenColumns("Cuid");
            }
        }
        public AccessTokenColumns ProviderId
        {
            get
            {
                return new AccessTokenColumns("ProviderId");
            }
        }
        public AccessTokenColumns ProviderName
        {
            get
            {
                return new AccessTokenColumns("ProviderName");
            }
        }
        public AccessTokenColumns Value
        {
            get
            {
                return new AccessTokenColumns("Value");
            }
        }
        public AccessTokenColumns CreatedBy
        {
            get
            {
                return new AccessTokenColumns("CreatedBy");
            }
        }
        public AccessTokenColumns ModifiedBy
        {
            get
            {
                return new AccessTokenColumns("ModifiedBy");
            }
        }
        public AccessTokenColumns Modified
        {
            get
            {
                return new AccessTokenColumns("Modified");
            }
        }
        public AccessTokenColumns Deleted
        {
            get
            {
                return new AccessTokenColumns("Deleted");
            }
        }
        public AccessTokenColumns Created
        {
            get
            {
                return new AccessTokenColumns("Created");
            }
        }



		protected internal Type TableType
		{
			get
			{
				return typeof(AccessToken);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}