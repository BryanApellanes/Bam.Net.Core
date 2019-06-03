using Bam.Net.CoreServices.ApplicationRegistration.Data;

namespace Bam.Net.Application.Verbs
{
    public static class Copy
    {
        public static CopyContext To(Machine machine)
        {
            CopyContext ctx = new CopyContext();
            return ctx.To(machine);
        }

        public static CopyContext To(this CopyContext ctx, Machine machine, string remotePath = null)
        {
            ctx.Machine = machine;
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
            ctx.LocalFilePath = path;
            return ctx;
        }

        public static CopyContext RemotePath(this CopyContext ctx, string remotePath)
        {
            ctx.RemoteFilePath = remotePath;
            return ctx;
        }
    }
}