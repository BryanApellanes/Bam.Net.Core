namespace Bam.Templates.Shell.Models
{
    public class DelegatorModel
    {
        public string NameSpace { get; set; }
        public string ProviderTypeName { get; set; }
        public DelegatorMethodModel[] DelegatorMethods { get; set; }
    }
}