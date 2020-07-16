using System;
using System.Linq;

namespace Bam.Net.Data.Repositories
{
    public class KeyedRepoData : CompositeKeyRepoData
    {
        public static implicit operator ulong(KeyedRepoData repoData)
        {
            return repoData.Key;
        }
        
        private ulong key = Convert.ToUInt64(0);
        public ulong Key
        {
            get
            {
                if (key == 0)
                {
                    key = Key();
                }

                return key;
            }
            set => key = value;
        }

        readonly object _saveLock = new object();
        public T SaveByKey<T>(IRepository repository) where T : KeyedRepoData, new()
        {
            T result = default(T);
            lock (_saveLock)
            {
                T existing = LoadExisting<T>(repository); 
                result = existing != null ? repository.Save<T>((T)existing.CopyProperties(this)) : repository.Save<T>((T)this);
            }

            return result;
        }
        
        /// <summary>
        /// Query for the KeyedAuditRepoData with the key matching the current instance then load it by its Uuid if it is found.
        /// </summary>
        /// <param name="repository"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T LoadByKey<T>(IRepository repository) where T : KeyedRepoData, new()
        {
            T queryResult = repository.Query<T>(new {Key = Key}).FirstOrDefault();
            if (queryResult != null)
            {
                return repository.Retrieve<T>(queryResult.Uuid);
            }

            return null;
        }
        
        private T LoadExisting<T>(IRepository repository) where T : KeyedRepoData, new()
        {
            T existing = LoadByKey<T>(repository);
            if (existing != null)
            {
                this.Uuid = existing.Uuid;
                this.Cuid = existing.Cuid;
            }

            return existing;
        }
    }
}