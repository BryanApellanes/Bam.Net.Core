namespace Bam.Net
{
    /// <summary>
    /// If the specified path starts with tilde (~), resolves ~ to the user's home directory.
    /// </summary>
    public class HomePath
    {
        public static implicit operator string(HomePath homePath)
        {
            return homePath.Resolve();
        }
        
        public HomePath(string path)
        {
            Path = path;
        }
        
        public string Path { get; private set; }
        
        public string Resolve()
        {
            string result = Path;
            if (result.StartsWith("~/"))
            {
                result = System.IO.Path.Combine(BamHome.UserHome, result.TruncateFront(2));
            }

            return result;
        }
    }
}