using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.ServiceProxy.Secure
{
    public class ConfigurationColumns: QueryFilter<ConfigurationColumns>, IFilterToken
    {
        public ConfigurationColumns() { }
        public ConfigurationColumns(string columnName, bool isForeignKey = false)
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
        
		public ConfigurationColumns KeyColumn => new ConfigurationColumns("Id");

        public ConfigurationColumns Id => new ConfigurationColumns("Id");
        public ConfigurationColumns Uuid => new ConfigurationColumns("Uuid");
        public ConfigurationColumns Cuid => new ConfigurationColumns("Cuid");
        public ConfigurationColumns Name => new ConfigurationColumns("Name");

        public ConfigurationColumns ApplicationId => new ConfigurationColumns("ApplicationId", true);

		public Type DaoType => typeof(Configuration);

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}