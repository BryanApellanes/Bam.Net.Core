using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.Encryption
{
    public class VaultKeyColumns: QueryFilter<VaultKeyColumns>, IFilterToken
    {
        public VaultKeyColumns() { }
        public VaultKeyColumns(string columnName, bool isForeignKey = false)
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
        
		public VaultKeyColumns KeyColumn => new VaultKeyColumns("Id");

        public VaultKeyColumns Id => new VaultKeyColumns("Id");
        public VaultKeyColumns Uuid => new VaultKeyColumns("Uuid");
        public VaultKeyColumns Cuid => new VaultKeyColumns("Cuid");
        public VaultKeyColumns RsaKey => new VaultKeyColumns("RsaKey");
        public VaultKeyColumns Password => new VaultKeyColumns("Password");


        public VaultKeyColumns VaultId => new VaultKeyColumns("VaultId", true);

		public Type DaoType => typeof(VaultKey);

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}