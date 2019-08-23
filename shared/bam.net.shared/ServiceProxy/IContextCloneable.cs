namespace Bam.Net.ServiceProxy
{
    public interface IContextCloneable
    {
        object Clone(IHttpContext context);
    }
}