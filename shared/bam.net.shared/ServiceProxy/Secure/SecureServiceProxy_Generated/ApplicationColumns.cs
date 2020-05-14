using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Bam.Net.Data;

namespace Bam.Net.ServiceProxy.Secure
{
    public class ApplicationColumns: QueryFilter<ApplicationColumns>, IFilterToken
    {
        public ApplicationColumns() { }
        public ApplicationColumns(string columnName, bool isForeignKey = false)
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
        
		public ApplicationColumns KeyColumn => new ApplicationColumns("Id");

        public ApplicationColumns Id => new ApplicationColumns("Id");
        public ApplicationColumns Uuid => new ApplicationColumns("Uuid");
        public ApplicationColumns Cuid => new ApplicationColumns("Cuid");
        public ApplicationColumns Name => new ApplicationColumns("Name");


		public Type DaoType => typeof(Application);

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}