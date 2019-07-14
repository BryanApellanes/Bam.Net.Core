using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

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
            CompositeKeyAlgorithm = HashAlgorithms.SHA256;
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

        public HashAlgorithms CompositeKeyAlgorithm { get; set; }
        
        ulong _key;
        public ulong CompositeKey
        {
            get
            {
                if (_key == 0 || _key != GetULongKeyHash())
                {
                    _key = GetULongKeyHash();
                }

                return _key;
            }
            set { _key = value; }
        }

        string _compositeKeyString;
        public string CompositeKeyString
        {
            get
            {
                if (string.IsNullOrEmpty(_compositeKeyString))
                {
                    _compositeKeyString = GetCompositeKeyString(CompositeKeyAlgorithm);
                }

                return _compositeKeyString;
            }
            set { _compositeKeyString = value; }
        }
        
        public string GetCompositeKeyString(HashAlgorithms algorithm = HashAlgorithms.SHA256)
        {
            if (algorithm == HashAlgorithms.Invalid)
            {
                algorithm = CompositeKeyAlgorithm;
            }

            return CompositeKeyHashProvider.GetStringKeyHash(this, PropertyDelimiter, algorithm);
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
