namespace Bam.Net.Data
{
    /// <summary>
    /// A class that represents the Universal Deterministic Identifier for an object instance.
    /// </summary>
    public class UniversalDeterministicIdResolver: IUniversalIdResolver
    {
        public UniversalDeterministicIdResolver(object data)
        {
            this.Data = data;
        }
        
        public object Data { get; set; }
        public ulong GetId(object data)
        {
            throw new System.NotImplementedException();
        }
    }
}