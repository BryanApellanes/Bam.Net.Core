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
            
            Args.Throw<InvalidOperationException>("Cannot resolve key for specified data.  Make sure that the type of the specified data implements either {0} or {1}; alternatively extend {2} to use the Cuid as the instance key.", nameof(IHasKeyHash), nameof(IHasKey), nameof(RepoData));
            return string.Empty;
        }
    }
}
