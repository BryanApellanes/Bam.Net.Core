using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Data.Dynamic;
using Bam.Net.Logging;
using Bam.Net.Services.DataReplication;

namespace Bam.Net.Data.Repositories
{
    /// <summary>
    /// An abstract base class defining common
    /// properties for any object you may wish to 
    /// save in a Repository
    /// </summary>
    [Serializable]
	public abstract class RepoData
	{
        /// <summary>
        /// The identifier for the current instance.  This should be
        /// considered a "local" id, meaning it identifies the instance
        /// from the current repository of the current process.  This value
        /// may be different for the same instance in a different process.
        /// For universal identity use Uuid.
        /// </summary>
        [Key]
		public ulong Id { get; set; }
        
        private DateTime? _created;
        /// <summary>
        /// The time that the Created property
        /// was first referenced prior to persisting
        /// the object instance
        /// </summary>
        public DateTime? Created
        {
            get
            {
                if (_created == null)
                {
                    _created = DateTime.UtcNow;
                }
                return _created;
            }
            set { _created = value; }
        }
        
        string _uuid;
        /// <summary>
        /// The universally unique identifier.  While this value should be
        /// universally unique, a very small possibility exists of collisions
        /// when generating Uuids concurrently across multiple threads and/or
        /// processes.  To confidently identify a unique data instance use a
        /// combination of Uuid and/or Cuid.  See Cuid.
        /// </summary>
        public string Uuid
        {
            get
            {
                if (string.IsNullOrEmpty(_uuid))
                {
                    _uuid = Guid.NewGuid().ToString();
                }
                return _uuid;
            }
            set
            {
                _uuid = value;
            }
        }
        
        string _cuid;
        /// <summary>
        /// The collision resistant unique identifier.
        /// </summary>
        public string Cuid
        {
            get
            {
                if (string.IsNullOrEmpty(_cuid))
                {
                    _cuid = NCuid.Cuid.Generate();
                }
                return _cuid;
            }
            set
            {
                _cuid = value;
            }
        }

        /// <summary>
        /// Does a query for an instance of the specified
        /// generic type T having properties who's values
        /// match those of the current instance; may return null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repo"></param>
        /// <param name="propertyNames"></param>
        /// <returns></returns>
        public virtual T QueryFirstOrDefault<T>(IRepository repo, params string[] propertyNames) where T : RepoData, new()
        {
            ValidatePropertyNamesOrDie(propertyNames);
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            propertyNames.Each(new { Parameters = parameters, Instance = this }, (ctx, pn) =>
            {
                ctx.Parameters.Add(pn, ReflectionExtensions.Property(ctx.Instance, pn));
            });
            T instance = repo.Query<T>(parameters).FirstOrDefault();
            return instance;
        }

        public override bool Equals(object obj)
        {
            if (obj is RepoData o)
            {
                return o.Uuid.Equals(Uuid) && o.Cuid.Equals(Cuid);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Uuid.GetHashCode() + Cuid.GetHashCode();
        }

        public virtual RepoData Save(IRepository repo)
        {
            return (RepoData)repo.Save((object)this);
        }

        public bool GetIsPersisted()
        {
            return IsPersisted;
        }

        public bool GetIsPersisted(out IRepository repo)
        {
            repo = Repository;
            return IsPersisted;
        }

        protected void ValidatePropertyNamesOrDie(params string[] propertyNames)
        {
            propertyNames.Each(new { Instance = this }, (ctx, pn) =>
            {
                Args.ThrowIf(!ReflectionExtensions.HasProperty(ctx.Instance, pn), "Specified property ({0}) was not found on instance of type ({1})", pn, ctx.Instance.GetType().Name);
            });
        }
        
        protected internal bool IsPersisted { get; set; }
        protected internal IRepository Repository { get; set; } // gets set by Repository.Save

        public static object GetInstanceId(object instance, UniversalIdentifiers universalIdentifier = UniversalIdentifiers.Cuid)
        {
            Args.ThrowIfNull(instance);
            if (!(instance is RepoData repoData))
            {
                Log.Warn("Getting instance id but specified object instance is not of type {0}: {1}", nameof(RepoData),
                    instance.ToString());
            }

            switch (universalIdentifier)
            {
                case UniversalIdentifiers.Uuid:
                    if (instance.HasProperty("Uuid"))
                    {
                        return instance.Property("Uuid");
                    }
                    break;
                case UniversalIdentifiers.Cuid:
                    if (instance.HasProperty("Cuid"))
                    {
                        return instance.Property("Cuid");
                    }
                    break;
                case UniversalIdentifiers.CKey:
                    if (instance.HasProperty("CompositeKeyId"))
                    {
                        if (!(instance is CompositeKeyAuditRepoData))
                        {
                            Log.Warn("Getting CompositeKeyId as instance id but specified object instance is not of type {0}: {1}", nameof(CompositeKeyAuditRepoData), instance.ToString());
                        }
                        return instance.Property("CompositeKey");
                    }else if (instance.HasProperty("Key"))
                    {
                        if (!(instance is KeyedAuditRepoData))
                        {
                            Log.Warn("Getting Key property as instance id but specified object instance is not of type {0}: {1}", nameof(KeyedAuditRepoData), instance.ToString());
                        }

                        return instance.Property("Key");
                    }
                    break;
            }
            
            return null;
        }
    }
}
