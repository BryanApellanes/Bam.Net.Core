using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Bam.Net;
using Bam.Net.CommandLine;
using Bam.Net.Data;
using Bam.Net.Logging;
using Bam.Net.Testing;
using Bam.Shell;

namespace Bam.Shell
{
    [Serializable]
    public class ExternalProvider: ShellProvider
    {
        [ArgZero("menu")]
        public void Menu()
        {
            string assemblyPath =
                GetArgument("assembly", "Please enter the path to the assembly to show a console menu for");
            
            FileInfo assemblyFile = new FileInfo(assemblyPath);
            ShowMenu(Assembly.LoadFile(assemblyFile.FullName), new ConsoleMenu[]{}, assemblyPath);
            Exit(0);
        }

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

        public override void Set(Action<string> output = null, Action<string> error = null)
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