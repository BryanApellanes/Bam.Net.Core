using System;
using System.Collections.Generic;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.CoreServices.Auth.Data.Dao
{
    public class AuthProviderSettingsColumns: QueryFilter<AuthProviderSettingsColumns>, IFilterToken
    {
        public AuthProviderSettingsColumns() { }
        public AuthProviderSettingsColumns(string columnName)
            : base(columnName)
        { }
		
		public AuthProviderSettingsColumns KeyColumn
		{
			get
			{
				return new AuthProviderSettingsColumns("Id");
			}
		}	

        public AuthProviderSettingsColumns Id
        {
            get
            {
                return new AuthProviderSettingsColumns("Id");
            }
        }
        public AuthProviderSettingsColumns Uuid
        {
            get
            {
                return new AuthProviderSettingsColumns("Uuid");
            }
        }
        public AuthProviderSettingsColumns Cuid
        {
            get
            {
                return new AuthProviderSettingsColumns("Cuid");
            }
        }
        public AuthProviderSettingsColumns ApplicationName
        {
            get
            {
                return new AuthProviderSettingsColumns("ApplicationName");
            }
        }
        public AuthProviderSettingsColumns ApplicationIdentifier
        {
            get
            {
                return new AuthProviderSettingsColumns("ApplicationIdentifier");
            }
        }
        public AuthProviderSettingsColumns ProviderName
        {
            get
            {
                return new AuthProviderSettingsColumns("ProviderName");
            }
        }
        public AuthProviderSettingsColumns State
        {
            get
            {
                return new AuthProviderSettingsColumns("State");
            }
        }
        public AuthProviderSettingsColumns Code
        {
            get
            {
                return new AuthProviderSettingsColumns("Code");
            }
        }
        public AuthProviderSettingsColumns ClientId
        {
            get
            {
                return new AuthProviderSettingsColumns("ClientId");
            }
        }
        public AuthProviderSettingsColumns ClientSecret
        {
            get
            {
                return new AuthProviderSettingsColumns("ClientSecret");
            }
        }
        public AuthProviderSettingsColumns AuthorizationUrl
        {
            get
            {
                return new AuthProviderSettingsColumns("AuthorizationUrl");
            }
        }
        public AuthProviderSettingsColumns AuthorizationCallbackEndpoint
        {
            get
            {
                return new AuthProviderSettingsColumns("AuthorizationCallbackEndpoint");
            }
        }
        public AuthProviderSettingsColumns AuthorizationEndpointFormat
        {
            get
            {
                return new AuthProviderSettingsColumns("AuthorizationEndpointFormat");
            }
        }
        public AuthProviderSettingsColumns AuthorizationCallbackEndpointFormat
        {
            get
            {
                return new AuthProviderSettingsColumns("AuthorizationCallbackEndpointFormat");
            }
        }
        public AuthProviderSettingsColumns Version
        {
            get
            {
                return new AuthProviderSettingsColumns("Version");
            }
        }
        public AuthProviderSettingsColumns AccessToken
        {
            get
            {
                return new AuthProviderSettingsColumns("AccessToken");
            }
        }
        public AuthProviderSettingsColumns CompositeKeyId
        {
            get
            {
                return new AuthProviderSettingsColumns("CompositeKeyId");
            }
        }
        public AuthProviderSettingsColumns CompositeKey
        {
            get
            {
                return new AuthProviderSettingsColumns("CompositeKey");
            }
        }
        public AuthProviderSettingsColumns CreatedBy
        {
            get
            {
                return new AuthProviderSettingsColumns("CreatedBy");
            }
        }
        public AuthProviderSettingsColumns ModifiedBy
        {
            get
            {
                return new AuthProviderSettingsColumns("ModifiedBy");
            }
        }
        public AuthProviderSettingsColumns Modified
        {
            get
            {
                return new AuthProviderSettingsColumns("Modified");
            }
        }
        public AuthProviderSettingsColumns Deleted
        {
            get
            {
                return new AuthProviderSettingsColumns("Deleted");
            }
        }
        public AuthProviderSettingsColumns Created
        {
            get
            {
                return new AuthProviderSettingsColumns("Created");
            }
        }



		protected internal Type TableType
		{
			get
			{
				return typeof(AuthProviderSettings);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}