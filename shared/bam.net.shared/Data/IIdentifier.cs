namespace Bam.Net.Data
{
    public interface IIdentifier
    {
        ulong GetDaoId(Dao dao);
        ulong GetCompositeKey(object obj);
        ulong GetUdi(object obj);
    }
}