using System;
using Bam.Net.Data.Repositories;
using Bam.Net.Services.DataReplication;

namespace Bam.Net.Data
{
    public class DaoId : QueryValue, IIdentifier
    {
        public DaoId(object value) : base(value)
        {
            IdentifierName = "Id";
        }
        
        /// <summary>
        /// The name of the property or column that represents the Dao's identifier, this is typically "Id".
        /// </summary>
        public string IdentifierName { get; set; }

        public override object GetStoredValue()
        {
            return GetRawValue();
        }

        public override object GetValue()
        {
            return GetRawValue();
        }

        public ulong GetDaoId(Dao dao)
        {
            Args.ThrowIfNull(dao, "dao");
            Args.ThrowIfNull(dao.IdValue, "dao.IdValue");
            return dao.IdValue.Value;
        }

        public ulong GetCompositeKey(object obj)
        {
            return CompositeKeyHashProvider.GetUniversalDeterministicId(obj);
        }

        public ulong GetUdi(object obj)
        {
            if (obj is Dao dao)
            {
                
            }
            
            throw new NotImplementedException();
        }
    }
}