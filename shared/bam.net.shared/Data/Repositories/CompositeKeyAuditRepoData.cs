using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.Data.Repositories
{
    /// <summary>
    /// The base class to extend for any class whose identity is determined
    /// by multiple properties adorned with CompositeKeyAttribute.
    /// </summary>
    /// <seealso cref="Bam.Net.Data.Repositories.AuditRepoData" />
    /// <seealso cref="Bam.Net.Data.Repositories.IHasKeyHash" />
    [Serializable]
    public abstract class CompositeKeyAuditRepoData : AuditRepoData, IHasKeyHash
    {
        public CompositeKeyAuditRepoData()
        {
            PropertyDelimiter = "\r\n";
        }

        string[] _compositeKeyProperties;
        public virtual string[] CompositeKeyProperties
        {
            get
            {
                if (_compositeKeyProperties == null)
                {
                    _compositeKeyProperties = CompositeKeyHashProvider.GetCompositeKeyProperties(GetType());
                }
                return _compositeKeyProperties;
            }
            set
            {
                _compositeKeyProperties = value;
            }
        }

        ulong _key;

        public ulong Key
        {
            get
            {
                if (_key == 0)
                {
                    return GetULongKeyHash();
                }

                return _key;
            }
            set { _key = value; }
        }

        public int GetIntKeyHash()
        {
            return CompositeKeyHashProvider.GetIntKeyHash(this, PropertyDelimiter, CompositeKeyProperties);
        }

        public long GetLongKeyHash()
        {
            return CompositeKeyHashProvider.GetLongKeyHash(this, PropertyDelimiter, CompositeKeyProperties);
        }

        public ulong GetULongKeyHash()
        {
            return CompositeKeyHashProvider.GetULongKeyHash(this, PropertyDelimiter, CompositeKeyProperties);
        }

        protected string PropertyDelimiter { get; set; }

        public override int GetHashCode()
        {
            return GetIntKeyHash();
        }

        public override bool Equals(object obj)
        {
            if (obj is CompositeKeyAuditRepoData o)
            {
                return o.GetHashCode().Equals(GetHashCode());
            }
            return false;
        }
    }
}
