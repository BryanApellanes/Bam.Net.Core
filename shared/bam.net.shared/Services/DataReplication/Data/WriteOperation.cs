using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.Services.DataReplication.Data
{
    public abstract class WriteOperation: Operation
    {  
        public OperationIntent Intent { get; set; }

        /// <summary>
        /// The properties to write
        /// </summary>
        public virtual List<DataProperty> Properties { get; set; }

        public override object Execute(IDistributedRepository repo)
        {
            WriteEvent writeEvent = this.CopyAs<WriteEvent>();            
            Commit(repo, writeEvent);
            Any?.Invoke(this, new OperationEventArgs { WriteEvent = writeEvent });
            return writeEvent;
        }

        protected void Commit(IDistributedRepository repo, WriteEvent writeEvent)
        {
            repo.Save(SaveOperation.For(writeEvent));
        }

        public static event EventHandler Any;

        public static DataPoint GetDataPoint(object instance)
        {
            return DataPoint.FromInstance(instance);
        } 
        
        protected static List<DataProperty> GetDataProperties(object instance)
        {
            DataPoint dataPoint = GetDataPoint(instance);
            return dataPoint.DataPropertySet.ToList();
        }
    }
}
