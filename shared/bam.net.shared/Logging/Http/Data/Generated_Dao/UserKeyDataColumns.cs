using System;
using System.Collections.Generic;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.Logging.Http.Data.Dao
{
    public class UserKeyDataColumns: QueryFilter<UserKeyDataColumns>, IFilterToken
    {
        public UserKeyDataColumns() { }
        public UserKeyDataColumns(string columnName)
            : base(columnName)
        { }
		
		public UserKeyDataColumns KeyColumn
		{
			get
			{
				return new UserKeyDataColumns("Id");
			}
		}	

        public UserKeyDataColumns Id
        {
            get
            {
                return new UserKeyDataColumns("Id");
            }
        }
        public UserKeyDataColumns Uuid
        {
            get
            {
                return new UserKeyDataColumns("Uuid");
            }
        }
        public UserKeyDataColumns Cuid
        {
            get
            {
                return new UserKeyDataColumns("Cuid");
            }
        }
        public UserKeyDataColumns UserKey
        {
            get
            {
                return new UserKeyDataColumns("UserKey");
            }
        }
        public UserKeyDataColumns UserName
        {
            get
            {
                return new UserKeyDataColumns("UserName");
            }
        }
        public UserKeyDataColumns Created
        {
            get
            {
                return new UserKeyDataColumns("Created");
            }
        }



		protected internal Type TableType
		{
			get
			{
				return typeof(UserKeyData);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}