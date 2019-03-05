using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;

namespace Bam.Net.Presentation
{
    public class BamTestPageModel :BamPageModel
    {
        public BamTestPageModel(IHostingEnvironment hostingEnvironment, ApplicationModel applicationModel) : base(hostingEnvironment, applicationModel)
        {
            TestFileNames = new List<string>();
        }

        public BamTestPageModel(IHostingEnvironment hostingEnvironment, ApplicationModel applicationModel, params string[] extensionsToLoad) : base(hostingEnvironment, applicationModel, extensionsToLoad)
        {
            TestFileNames = new List<string>();
        }
        
        public List<string> TestFileNames { get; set; }
    }
}