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
    public abstract class CompositeKeyAuditRepoData : AuditRepoData, IHasKeyHash, IHasKey
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
        
        ulong _keyId;
        public ulong CompositeKeyId
        {
            get
            {
                if (_keyId == 0 || _keyId != GetULongKeyHash())
                {
                    _keyId = GetULongKeyHash();
                }

                return _keyId;
            }
            set => _keyId = value;
        }

        string _compositeKey;
        public string CompositeKey
        {
            get
            {
                if (string.IsNullOrEmpty(_compositeKey))
                {
                    _compositeKey = GetCompositeKeyString(CompositeKeyAlgorithm);
                }

                return _compositeKey;
            }
            set => _compositeKey = value;
        }

        public ulong Key()
        {
            return CompositeKeyId;
        }
        
        public bool ExistsIn<T>(IRepository repository) where T : CompositeKeyAuditRepoData, new()
        {
            return ExistsIn<T>(repository, out T ignore);
        }
        
        public bool ExistsIn<T>(IRepository repository, out T existing) where T: CompositeKeyAuditRepoData, new()
        {
            existing = LoadByCompositeKey<T>(repository);
            return existing != null;
        }
        
        public virtual T LoadByCompositeKey<T>(IRepository repository) where T : CompositeKeyAuditRepoData, new()
        {
            return repository.Query<T>(new {CompositeKey = CompositeKey}).FirstOrDefault();
        }

        public virtual T LoadByCompositeKeyId<T>(IRepository repository) where T : CompositeKeyAuditRepoData, new()
        {
            return repository.Query<T>(new {CompositeKeyId = CompositeKeyId}).FirstOrDefault();
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
