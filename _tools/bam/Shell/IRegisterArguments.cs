namespace Bam.Shell
{
    public interface IRegisterArguments
    {
        string[] RawArguments { get; }
        void RegisterArguments(string[] args);
    }
}