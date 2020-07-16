namespace Bam.Net.Data
{
    public class UniversalIdResolver: IUniversalIdResolver
    {
        public UniversalIdResolver(object data)
        {
            Data = data;
        }
        public object Data { get; set; }
        
        public ulong GetId(object data)
        {
            throw new System.NotImplementedException();
        }
    }
}