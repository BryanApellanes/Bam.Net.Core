using Bam.Net.Application.Network;
using Bam.Net.CoreServices.ApplicationRegistration.Data;

namespace Bam.Net.Application.Verbs
{
    public static class Copy
    {
        public static CopyContext To(Remote machine)
        {
            CopyContext ctx = new CopyContext();
            return ctx.To(machine);
        }

        public static CopyContext To(this CopyContext ctx, Remote machine, string remotePath = null)
        {
            ctx.Remote = machine;
            if (!string.IsNullOrEmpty(remotePath))
            {
                return ctx.RemotePath(remotePath);
            }
            return ctx;
        }
        
        public static CopyContext LocalFile(string localPath)
        {
            CopyContext ctx = new CopyContext();
            return ctx.LocalFile(localPath);
        }
        
        public static CopyContext LocalFile(this CopyContext ctx, string path)
        {
            ctx.LocalPath = path;
            return ctx;
        }

        public static CopyContext RemotePath(this CopyContext ctx, string remotePath)
        {
            ctx.RemotePath = remotePath;
            return ctx;
        }
    }
}