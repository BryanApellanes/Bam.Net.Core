using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.ServiceProxy.Secure
{
    public class SecureSessionColumns: QueryFilter<SecureSessionColumns>, IFilterToken
    {
        public SecureSessionColumns() { }
        public SecureSessionColumns(string columnName, bool isForeignKey = false)
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
        
		public SecureSessionColumns KeyColumn => new SecureSessionColumns("Id");

        public SecureSessionColumns Id => new SecureSessionColumns("Id");
        public SecureSessionColumns Uuid => new SecureSessionColumns("Uuid");
        public SecureSessionColumns Cuid => new SecureSessionColumns("Cuid");
        public SecureSessionColumns Identifier => new SecureSessionColumns("Identifier");
        public SecureSessionColumns AsymmetricKey => new SecureSessionColumns("AsymmetricKey");
        public SecureSessionColumns SymmetricKey => new SecureSessionColumns("SymmetricKey");
        public SecureSessionColumns SymmetricIV => new SecureSessionColumns("SymmetricIV");
        public SecureSessionColumns CreationDate => new SecureSessionColumns("CreationDate");
        public SecureSessionColumns TimeOffset => new SecureSessionColumns("TimeOffset");
        public SecureSessionColumns LastActivity => new SecureSessionColumns("LastActivity");
        public SecureSessionColumns IsActive => new SecureSessionColumns("IsActive");

        public SecureSessionColumns ApplicationId => new SecureSessionColumns("ApplicationId", true);

		public Type DaoType => typeof(SecureSession);

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}