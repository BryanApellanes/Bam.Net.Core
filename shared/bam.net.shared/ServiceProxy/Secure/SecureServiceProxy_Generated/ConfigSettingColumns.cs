using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.ServiceProxy.Secure
{
    public class ConfigSettingColumns: QueryFilter<ConfigSettingColumns>, IFilterToken
    {
        public ConfigSettingColumns() { }
        public ConfigSettingColumns(string columnName, bool isForeignKey = false)
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
        
		public ConfigSettingColumns KeyColumn => new ConfigSettingColumns("Id");

        public ConfigSettingColumns Id => new ConfigSettingColumns("Id");
        public ConfigSettingColumns Uuid => new ConfigSettingColumns("Uuid");
        public ConfigSettingColumns Cuid => new ConfigSettingColumns("Cuid");
        public ConfigSettingColumns Key => new ConfigSettingColumns("Key");
        public ConfigSettingColumns Value => new ConfigSettingColumns("Value");

        public ConfigSettingColumns ConfigurationId => new ConfigSettingColumns("ConfigurationId", true);

		public Type DaoType => typeof(ConfigSetting);

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}