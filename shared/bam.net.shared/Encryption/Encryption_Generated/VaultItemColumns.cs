using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.Encryption
{
    public class VaultItemColumns: QueryFilter<VaultItemColumns>, IFilterToken
    {
        public VaultItemColumns() { }
        public VaultItemColumns(string columnName, bool isForeignKey = false)
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
        
		public VaultItemColumns KeyColumn => new VaultItemColumns("Id");

        public VaultItemColumns Id => new VaultItemColumns("Id");
        public VaultItemColumns Uuid => new VaultItemColumns("Uuid");
        public VaultItemColumns Cuid => new VaultItemColumns("Cuid");
        public VaultItemColumns Key => new VaultItemColumns("Key");
        public VaultItemColumns Value => new VaultItemColumns("Value");


        public VaultItemColumns VaultId => new VaultItemColumns("VaultId", true);

		public Type DaoType => typeof(VaultItem);

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}