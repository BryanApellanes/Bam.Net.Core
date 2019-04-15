namespace Bam.Shell.ShellGeneration.Configuration
{
    public class ShellGenConfig
    {
        public string BaseProviderNamespace { get; set; }
        public string BaseProviderTypeName { get; set; }
        public string ConcreteTypeName { get; set; }
        public string DelegatorNamespace { get; set; }
        public string WriteTo { get; set; }
    }
}