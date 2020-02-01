namespace Bam.Net
{
    public class UnixPath
    {
        public static implicit operator string(UnixPath unixPath)
        {
            return unixPath.Resolve();
        }
        
        public UnixPath(string path)
        {
            Path = path;
        }
        
        public string Path { get; set; }
        
        public string Resolve()
        {
            string result = Path;
            if (result.StartsWith("~/"))
            {
                result = System.IO.Path.Combine(BamPaths.UserHome, result.TruncateFront(2));
            }

            return result;
        }
    }
}