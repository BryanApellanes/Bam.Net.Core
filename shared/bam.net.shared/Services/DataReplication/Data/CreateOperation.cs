/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return result;
        }

    }
}
