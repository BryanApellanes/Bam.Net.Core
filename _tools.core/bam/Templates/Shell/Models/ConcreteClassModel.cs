namespace Bam.Templates.Shell.Models
{
    public class ConcreteClassModel
    {
        public string UsingNameSpaces { get; set; }
        public string NameSpace { get; set; }
        public string ConcreteTypeName { get; set; }
        public string BaseProviderTypeName { get; set; }
        public ConcreteMethodModel[] Methods { get; set; }
    }
}