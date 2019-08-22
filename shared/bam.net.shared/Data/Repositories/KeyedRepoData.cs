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
            set { key = value; }
        }

        object _saveLock = new object();
        public T SaveByKey<T>(IRepository repository) where T : KeyedRepoData, new()
        {
            lock (_saveLock)
            {
                T existing = repository.Query<T>(new {Key = Key}).FirstOrDefault() ?? repository.Save<T>((T)this);

                return existing;
            }
        }
    }
}