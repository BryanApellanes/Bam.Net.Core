namespace Bam.Templates.Shell.Models
{
    public class DelegatorClassModel
    {
        public string NameSpace { get; set; }
        public string BaseTypeName { get; set; }
        public DelegatorMethodModel[] Methods { get; set; }
    }
}