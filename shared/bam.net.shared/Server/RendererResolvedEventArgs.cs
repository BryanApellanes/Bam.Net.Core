using System;
using Bam.Net.ServiceProxy;

namespace Bam.Net.Server
{
    public class RendererResolvedEventArgs : EventArgs
    {
        public AppConf AppConf { get; set; }
        public IRequest Request { get; set; }
        public AppPageRenderer AppPageRenderer { get; set; }
    }
}