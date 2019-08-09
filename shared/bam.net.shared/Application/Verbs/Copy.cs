using System.IO;
using Bam.Net.Application.Network;
using Bam.Net.CoreServices.ApplicationRegistration.Data;

namespace Bam.Net.Application.Verbs
{
    public static class Copy
    {
        public static CopyContext To(string remoteHostName)
        {
            return To(Remote.For(remoteHostName));
        }
        
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
        
        public static CopyContext LocalPath(string localPath)
        {
            CopyContext ctx = new CopyContext();
            return ctx.LocalPath(localPath);
        }
        
        public static CopyContext LocalPath(this CopyContext ctx, string path)
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