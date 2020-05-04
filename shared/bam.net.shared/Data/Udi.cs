namespace Bam.Net.Data
{
    /// <summary>
    /// A class that represents the Universal Deterministic Identifier for an object instance.
    /// </summary>
    public class Udi
    {
        public Udi(object data)
        {
            this.Data = data;
        }
        
        public object Data { get; set; }
        public IIdentifier Identifier { get; set; }

        public ulong Value => Identifier.GetUdi(Data);
    }
}