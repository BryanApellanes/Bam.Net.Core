using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.Encryption
{
    public class VaultItemColumns: QueryFilter<VaultItemColumns>, IFilterToken
    {
        public VaultItemColumns() { }
        public VaultItemColumns(string columnName)
            : base(columnName)
        { }
        
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
                    PropertyInfo[] props = DaoType.GetProperties();
                    foreach (PropertyInfo propertyInfo in props)
                    {
                        if (propertyInfo.HasCustomAttributeOfType<ForeignKeyAttribute>(out ForeignKeyAttribute foreignKeyAttribute))
                        {
                            _isForeignKey = foreignKeyAttribute.Name.Equals(ColumnName);
                            break;
                        }
                    }
                }

                return _isForeignKey.Value;
            }
        }
        
		public VaultItemColumns KeyColumn
		{
			get
			{
				return new VaultItemColumns("Id");
			}
		}	

        public VaultItemColumns Id
        {
            get
            {
                return new VaultItemColumns("Id");
            }
        }
        public VaultItemColumns Uuid
        {
            get
            {
                return new VaultItemColumns("Uuid");
            }
        }
        public VaultItemColumns Cuid
        {
            get
            {
                return new VaultItemColumns("Cuid");
            }
        }
        public VaultItemColumns Key
        {
            get
            {
                return new VaultItemColumns("Key");
            }
        }
        public VaultItemColumns Value
        {
            get
            {
                return new VaultItemColumns("Value");
            }
        }


        public VaultItemColumns VaultId
        {
            get
            {
                return new VaultItemColumns("VaultId");
            }
        }

		public Type DaoType
		{
			get
			{
				return typeof(VaultItem);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}