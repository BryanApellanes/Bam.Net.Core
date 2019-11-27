using System;
using System.Collections.Generic;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.Logging.Http.Data.Dao
{
    public class ContentNotFoundDataColumns: QueryFilter<ContentNotFoundDataColumns>, IFilterToken
    {
        public ContentNotFoundDataColumns() { }
        public ContentNotFoundDataColumns(string columnName)
            : base(columnName)
        { }
		
		public ContentNotFoundDataColumns KeyColumn
		{
			get
			{
				return new ContentNotFoundDataColumns("Id");
			}
		}	

        public ContentNotFoundDataColumns Id
        {
            get
            {
                return new ContentNotFoundDataColumns("Id");
            }
        }
        public ContentNotFoundDataColumns Uuid
        {
            get
            {
                return new ContentNotFoundDataColumns("Uuid");
            }
        }
        public ContentNotFoundDataColumns Cuid
        {
            get
            {
                return new ContentNotFoundDataColumns("Cuid");
            }
        }
        public ContentNotFoundDataColumns RequestId
        {
            get
            {
                return new ContentNotFoundDataColumns("RequestId");
            }
        }
        public ContentNotFoundDataColumns ResponderName
        {
            get
            {
                return new ContentNotFoundDataColumns("ResponderName");
            }
        }
        public ContentNotFoundDataColumns RequestDataId
        {
            get
            {
                return new ContentNotFoundDataColumns("RequestDataId");
            }
        }
        public ContentNotFoundDataColumns CheckedPaths
        {
            get
            {
                return new ContentNotFoundDataColumns("CheckedPaths");
            }
        }
        public ContentNotFoundDataColumns Key
        {
            get
            {
                return new ContentNotFoundDataColumns("Key");
            }
        }
        public ContentNotFoundDataColumns Created
        {
            get
            {
                return new ContentNotFoundDataColumns("Created");
            }
        }



		protected internal Type TableType
		{
			get
			{
				return typeof(ContentNotFoundData);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}