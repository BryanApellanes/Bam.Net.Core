namespace Bam.Net
{
    public class ReferenceInfo
    {
        public ReferenceKind Kind { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        
        protected internal ProjectItemGroupPackageReference PackageReference { get; set; }
        protected internal ProjectItemGroupProjectReference ProjectReference { get; set; }

        protected internal Project Project { get; set; }
    }
}