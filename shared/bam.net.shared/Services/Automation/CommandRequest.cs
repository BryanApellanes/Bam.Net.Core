namespace Bam.Net.Services.Automation
{
    public class CommandRequest
    {
        public string Command { get; set; }
        public string[] Arguments { get; set; }
        
        /// <summary>
        /// The url to post output to as it is received.
        /// </summary>
        public string OutputPostUrl { get; set; }
        
        /// <summary>
        /// The url to post error output to as it is received.
        /// </summary>
        public string ErrorPostUrl { get; set; }
    }
}