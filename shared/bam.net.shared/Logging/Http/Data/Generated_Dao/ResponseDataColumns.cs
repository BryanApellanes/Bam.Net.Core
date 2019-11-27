using System;
using System.Collections.Generic;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.Logging.Http.Data.Dao
{
    public class ResponseDataColumns: QueryFilter<ResponseDataColumns>, IFilterToken
    {
        public ResponseDataColumns() { }
        public ResponseDataColumns(string columnName)
            : base(columnName)
        { }
		
		public ResponseDataColumns KeyColumn
		{
			get
			{
				return new ResponseDataColumns("Id");
			}
		}	

        public ResponseDataColumns Id
        {
            get
            {
                return new ResponseDataColumns("Id");
            }
        }
        public ResponseDataColumns Uuid
        {
            get
            {
                return new ResponseDataColumns("Uuid");
            }
        }
        public ResponseDataColumns Cuid
        {
            get
            {
                return new ResponseDataColumns("Cuid");
            }
        }
        public ResponseDataColumns RequestId
        {
            get
            {
                return new ResponseDataColumns("RequestId");
            }
        }
        public ResponseDataColumns CheckedPaths
        {
            get
            {
                return new ResponseDataColumns("CheckedPaths");
            }
        }
        public ResponseDataColumns ContentLength
        {
            get
            {
                return new ResponseDataColumns("ContentLength");
            }
        }
        public ResponseDataColumns ContentType
        {
            get
            {
                return new ResponseDataColumns("ContentType");
            }
        }
        public ResponseDataColumns RedirectLocation
        {
            get
            {
                return new ResponseDataColumns("RedirectLocation");
            }
        }
        public ResponseDataColumns StatusCode
        {
            get
            {
                return new ResponseDataColumns("StatusCode");
            }
        }
        public ResponseDataColumns StatusDescription
        {
            get
            {
                return new ResponseDataColumns("StatusDescription");
            }
        }
        public ResponseDataColumns Key
        {
            get
            {
                return new ResponseDataColumns("Key");
            }
        }
        public ResponseDataColumns Created
        {
            get
            {
                return new ResponseDataColumns("Created");
            }
        }



		protected internal Type TableType
		{
			get
			{
				return typeof(ResponseData);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}