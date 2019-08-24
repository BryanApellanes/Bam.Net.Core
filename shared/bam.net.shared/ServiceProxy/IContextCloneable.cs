namespace Bam.Net.ServiceProxy
{
    public interface IContextCloneable
    {
        object CloneInContext();
        object Clone(IHttpContext context);
    }
}