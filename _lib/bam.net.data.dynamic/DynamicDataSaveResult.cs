using Bam.Net.Data.Dynamic.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Data.Dynamic
{
    public class DynamicDataSaveResult
    {
        public DynamicDataSaveResult() { }

        public DynamicDataSaveResult(DynamicTypeDescriptor dynamicTypeDescriptor, DataInstance dataInstance)
        {
            DynamicTypeDescriptor = dynamicTypeDescriptor;
            DataInstances = new List<DataInstance>() { dataInstance };
        }

        public DynamicTypeDescriptor DynamicTypeDescriptor { get; set; }
        public List<DataInstance> DataInstances { get; set; }
    }
}
