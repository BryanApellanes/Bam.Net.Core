using System;
using Bam.Net.Services.DataReplication.Data;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class OperationNotSupportedException : Exception
    {
        public OperationNotSupportedException(Operation operation) : base(
            $"Specified operation not supported: {operation.ToJson(true)}")
        {
        }
    }
}