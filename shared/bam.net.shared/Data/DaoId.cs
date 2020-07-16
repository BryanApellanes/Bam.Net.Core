using System;
using Bam.Net.Data.Repositories;
using Bam.Net.Services.DataReplication;

namespace Bam.Net.Data
{
    public class DaoId : QueryValue
    {
        public DaoId(object value, QueryFilter filter) : base(value, filter)
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
            return dao.GetId().Value;
        }

        public IUniversalIdResolver GetUniversalIdentifier(Dao data)
        {
            return new UniversalIdResolver(data);
        }
        
        public ulong GetId(object obj)
        {
            if (obj is Dao dao)
            {
                return GetDaoId(dao);
            }
            throw new InvalidOperationException("The specified object must be a Dao instance.");
        }
    }
}