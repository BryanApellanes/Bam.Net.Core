namespace Bam.Net.Data
{
    public interface IUniversalIdResolver
    {
        ulong GetId(object data);
    }
}