﻿using Bam.Net.Testing;
using System;
using Bam.Net.Application.Verbs;

namespace Bam.Net.UserAccounts.Tests
{
    [Serializable]
    public partial class Program : CommandLineTestInterface
    {
        static void Main(string[] args)
        {
            ExecuteMain(args);
        }
    }
}
