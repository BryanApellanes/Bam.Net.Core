using System;

namespace Bam.Shell.Conf
{
    public class AppConfProvider : Bam.Shell.Conf.ConfProvider
    {
        public override void Get(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Set(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Print(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }
    }
}