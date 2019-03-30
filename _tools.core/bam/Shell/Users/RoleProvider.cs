using System;
using Bam.Shell;

namespace bam.Shell.Users
{
    public class RoleProvider: ShellProvider
    {
        public override void List(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Add(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Show(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Remove(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Run(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }
    }
}