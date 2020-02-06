using System;

namespace Bam.Net.Server
{
    public class RendererLoadedEventArgs : EventArgs
    {
        public AppConf AppConf { get; set; }
        public AppPageRenderer Renderer { get; set; }
    }
}