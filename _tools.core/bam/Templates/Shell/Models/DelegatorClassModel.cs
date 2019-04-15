namespace Bam.Templates.Shell.Models
{
    public class DelegatorClassModel
    {
        public string NameSpace { get; set; }
        public string BaseProviderTypeName { get; set; }
        public DelegatorMethodModel[] DelegatorMethods { get; set; }
    }
}