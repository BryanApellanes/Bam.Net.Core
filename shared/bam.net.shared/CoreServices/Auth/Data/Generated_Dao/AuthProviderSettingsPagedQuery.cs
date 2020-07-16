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
    public class AuthProviderSettingsPagedQuery: PagedQuery<AuthProviderSettingsColumns, AuthProviderSettings>
    { 
		public AuthProviderSettingsPagedQuery(AuthProviderSettingsColumns orderByColumn,AuthProviderSettingsQuery query, Database db = null) : base(orderByColumn, query, db) { }
    }
}