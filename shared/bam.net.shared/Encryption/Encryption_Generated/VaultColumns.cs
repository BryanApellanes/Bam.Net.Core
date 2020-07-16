using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.Encryption
{
    public class VaultColumns: QueryFilter<VaultColumns>, IFilterToken
    {
        public VaultColumns() { }
        public VaultColumns(string columnName, bool isForeignKey = false)
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
        
		public VaultColumns KeyColumn => new VaultColumns("Id");

        public VaultColumns Id => new VaultColumns("Id");
        public VaultColumns Uuid => new VaultColumns("Uuid");
        public VaultColumns Cuid => new VaultColumns("Cuid");
        public VaultColumns Name => new VaultColumns("Name");



		public Type DaoType => typeof(Vault);

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}