/*
	This file was generated and should not be modified directly
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Bam.Net.Data;

namespace Bam.Net.CoreServices.Auth.Data.Dao
{
    public class AccessTokenPagedQuery: PagedQuery<AccessTokenColumns, AccessToken>
    { 
		public AccessTokenPagedQuery(AccessTokenColumns orderByColumn,AccessTokenQuery query, Database db = null) : base(orderByColumn, query, db) { }
    }
}