using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.Services.DataReplication.Data
{
    [Serializable]
    public class SaveOperation : WriteOperation
    {        
        public override object Execute(IDistributedRepository repository)
        {
            return repository.Save(this);
        }

        public static SaveOperation For(object toSave)
        {
            Args.ThrowIfNull(toSave, "toSave");
            List<DataProperty> data = GetDataProperties(toSave);
            SaveOperation result = For<SaveOperation>(toSave.GetType());
            result.Properties = data;
            return result;
        }

    }
}
