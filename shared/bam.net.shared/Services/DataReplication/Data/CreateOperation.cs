/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Data;
using Bam.Net.Data.Repositories;
using Bam.Net.Logging;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;

namespace Bam.Net.Services.DataReplication.Data
{
    [Serializable]
    public class CreateOperation : WriteOperation
    {
        public CreateOperation()
        {
            Intent = OperationIntent.Create;
        }

        public UniversalIdentifiers IdentifierType { get; set; }  
        
        public override object Execute(IDistributedRepository repository)
        {
            repository.Create(this);
            return base.Execute(repository);
        }

        public static CreateOperation For(object toCreate, UniversalIdentifiers identifierType = UniversalIdentifiers.CKey)
        {
            List<DataProperty> data = GetDataProperties(toCreate);
            CreateOperation result = For<CreateOperation>(toCreate.GetType());
            result.IdentifierType = identifierType;
            result.Properties = data;
            result.IdentifierSetters[identifierType](toCreate);
            return result;
        }

        Dictionary<UniversalIdentifiers, Action<object>> _identifierSetters;

        private Dictionary<UniversalIdentifiers, Action<object>> IdentifierSetters
        {
            get
            {
                if (_identifierSetters == null)
                {
                    _identifierSetters = new Dictionary<UniversalIdentifiers, Action<object>>()
                    {
                        {UniversalIdentifiers.Uuid, (data) => { ForEachDataProperty(dp=> dp.InstanceIdentifier = Guid.NewGuid().ToString()); }},
                        {UniversalIdentifiers.Cuid, (data) => { ForEachDataProperty(dp=> dp.InstanceIdentifier = NCuid.Cuid.Generate());}},
                        {UniversalIdentifiers.CKey, (data) => { ForEachDataProperty(dp=> dp.InstanceIdentifier = GetKey(data));}}
                    };
                }

                return _identifierSetters;
            }
        }

        private void ForEachDataProperty(Action<DataProperty> dataPropertyAction)
        {
            foreach (DataProperty dataProperty in Properties)
            {
                dataPropertyAction(dataProperty);
            }
        }

        private string GetKey(object data)
        {
            Args.ThrowIfNull(data, "data");
            
            if (data is IHasKeyHash keyHash)
            {
                return keyHash.GetULongKeyHash().ToString();
            }

            if (data is IHasKey hasKey)
            {
                return hasKey.Key().ToString();
            }

            if (data is RepoData repoData)
            {
                return repoData.Cuid;
            }

            Log.Warn("The specified data doesn't implement {0} or {1}, and it doesn't extend {2}.  Checking for Cuid or Uuid property.", nameof(IHasKeyHash), nameof(IHasKey), nameof(RepoData));
            string key = data.Property<string>("Cuid", false);
            if (!string.IsNullOrEmpty(key))
            {
                Log.Info("Found Cuid property on specified data {0}", data.ToString());
                return key;
            }
            key = data.Property<string>("Uuid", false);
            if (!string.IsNullOrEmpty(key))
            {
                Log.Info("Found Uuid property on specified data {0}", data.ToString());
                return key;
            }
            if (data is Net.Data.Dao dao)
            {
                Args.Throw<InvalidOperationException>("Cannot resolve key for specified Dao instance; Did you mean to specify a RepoData instance instead?  Make sure that the type of the specified data implements either {0} or {1}; alternatively extend {2} to use the Cuid as the instance key.", nameof(IHasKeyHash), nameof(IHasKey), nameof(RepoData));
            }
            Args.Throw<InvalidOperationException>("Cannot resolve key for specified data.  Make sure that the type of the specified data implements either {0} or {1}; alternatively extend {2} to use the Cuid as the instance key.", nameof(IHasKeyHash), nameof(IHasKey), nameof(RepoData));
            return string.Empty;
        }
    }
}
