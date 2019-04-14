namespace Bam.Templates.Shell.Models
{
    public class ProviderBaseClassModel
    {
        public string NameSpace { get; set; }
        public string ProviderTypeName { get; set; }
        
        public ProviderBaseClassMethodModel[] Methods { get; set; }
    }
}