using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.ServiceProxy.Secure
{
    public class ApiKeyColumns: QueryFilter<ApiKeyColumns>, IFilterToken
    {
        public ApiKeyColumns() { }
        public ApiKeyColumns(string columnName, bool isForeignKey = false)
            : base(columnName)
        { 
            _isForeignKey = isForeignKey;
        }
        
        public bool IsKey()
        {
            return (bool)ColumnName?.Equals(KeyColumn.ColumnName);
        }

        private bool? _isForeignKey;
        public bool IsForeignKey
        {
            get
            {
                if (_isForeignKey == null)
                {
                    PropertyInfo prop = DaoType
                        .GetProperties()
                        .FirstOrDefault(pi => ((MemberInfo) pi)
                            .HasCustomAttributeOfType<ForeignKeyAttribute>(out ForeignKeyAttribute foreignKeyAttribute)
                                && foreignKeyAttribute.Name.Equals(ColumnName));
                        _isForeignKey = prop != null;
                }

                return _isForeignKey.Value;
            }
            set => _isForeignKey = value;
        }
        
		public ApiKeyColumns KeyColumn => new ApiKeyColumns("Id");

        public ApiKeyColumns Id => new ApiKeyColumns("Id");
        public ApiKeyColumns Uuid => new ApiKeyColumns("Uuid");
        public ApiKeyColumns Cuid => new ApiKeyColumns("Cuid");
        public ApiKeyColumns ClientId => new ApiKeyColumns("ClientId");
        public ApiKeyColumns SharedSecret => new ApiKeyColumns("SharedSecret");
        public ApiKeyColumns CreatedBy => new ApiKeyColumns("CreatedBy");
        public ApiKeyColumns CreatedAt => new ApiKeyColumns("CreatedAt");
        public ApiKeyColumns Confirmed => new ApiKeyColumns("Confirmed");
        public ApiKeyColumns Disabled => new ApiKeyColumns("Disabled");
        public ApiKeyColumns DisabledBy => new ApiKeyColumns("DisabledBy");

        public ApiKeyColumns ApplicationId => new ApiKeyColumns("ApplicationId", true);

		public Type DaoType => typeof(ApiKey);

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}