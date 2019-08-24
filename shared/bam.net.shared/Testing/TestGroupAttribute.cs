using System;
using System.Collections.Generic;

namespace Bam.Net.Testing
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TestGroupAttribute : Attribute
    {
        public TestGroupAttribute(params string[] groups)
        {
            Groups = new HashSet<string>(groups);
        }
        
        public HashSet<string> Groups { get; set; }
    }
}