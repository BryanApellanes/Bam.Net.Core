using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bam.Net.Data.Repositories
{

    /// <summary>
    /// Extend this class to define a type that uses multiple properties to determine
    /// persistence instance uniqueness.  Adorn key properties with the CompositeKey
    /// attribute.  Adds the Key property to CompositeKeyAuditRepoData.
    /// </summary>
    [Serializable]
    public abstract class KeyedAuditRepoData : CompositeKeyAuditRepoData
    {
        public static implicit operator ulong(KeyedAuditRepoData repoData)
        {
            return repoData.Key;
        }
        
        ulong key = Convert.ToUInt64(0);
        public ulong Key
        {
            get
            {
                if (key == 0)
                {
                    key = GetULongKeyHash();
                }
                return key;
            }
            set
            {
                key = value;
            }
        }

        readonly object _saveLock = new object();
        /// <summary>
        /// Save the current instance to the specified repository, first checking if an instance with the same key is already saved.
        /// </summary>
        /// <param name="repository"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T SaveByKey<T>(IRepository repository) where T : KeyedAuditRepoData, new()
        {
            T result = default(T);
            lock (_saveLock)
            {
                T existing = LoadByKey<T>(repository);
                result = existing != null ? repository.Save<T>((T)existing.CopyProperties(this)) : repository.Save<T>((T)this);
            }
            return result;
        }

        public T LoadByKey<T>(IRepository repository) where T : KeyedAuditRepoData, new()
        {
            T queryResult = repository.Query<T>(new {Key = Key}).FirstOrDefault();
            if (queryResult != null)
            {
                return repository.Retrieve<T>(queryResult.Uuid);
            }

            return null;
        }
    }
}
