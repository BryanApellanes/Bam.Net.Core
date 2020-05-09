using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.Encryption
{
    public class VaultColumns: QueryFilter<VaultColumns>, IFilterToken
    {
        public VaultColumns() { }
        public VaultColumns(string columnName)
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
        
		public VaultColumns KeyColumn
		{
			get
			{
				return new VaultColumns("Id");
			}
		}	

        public VaultColumns Id
        {
            get
            {
                return new VaultColumns("Id");
            }
        }
        public VaultColumns Uuid
        {
            get
            {
                return new VaultColumns("Uuid");
            }
        }
        public VaultColumns Cuid
        {
            get
            {
                return new VaultColumns("Cuid");
            }
        }
        public VaultColumns Name
        {
            get
            {
                return new VaultColumns("Name");
            }
        }



		public Type DaoType
		{
			get
			{
				return typeof(Vault);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}