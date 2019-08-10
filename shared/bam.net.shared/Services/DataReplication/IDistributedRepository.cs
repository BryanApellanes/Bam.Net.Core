/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Services.DataReplication.Data;

namespace Bam.Net.Services.DataReplication
{
    public interface IDistributedRepository
    {
        object Save(SaveOperation saveOperation);
        object Create(CreateOperation createOperation);
        object Retrieve(RetrieveOperation retrieveOperation);
        object Update(UpdateOperation updateOperation);
        bool Delete(DeleteOperation deleteOperation);
        IEnumerable<object> Query(QueryOperation queryOperation);

        ReplicationOperation Replicate(ReplicationOperation operation);

        IEnumerable<object> NextSet(ReplicationOperation operation);
    }
}
