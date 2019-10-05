using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bam.Net;
using Bam.Net.Application;

namespace Bam.Shell.Data
{
    public class EntryProvider : DataShellProvider
    {
        public override void RegisterArguments(string[] args)
        {
            base.RegisterArguments(args);
            AddValidArgument("Schema", "The name of the schema to act on.");
        }

        static string _defaultSchemaName;
        static object _defaultSchemaLock = new object();
        public static string DefaultSchemaName
        {
            get
            {
                return _defaultSchemaLock.DoubleCheckLock(ref _defaultSchemaName,
                    () => Config.Current["DefaultSchema"].Or("Public"));
            }
        }
        
        public override void New(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Get(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Set(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Del(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Find(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }


    }
}