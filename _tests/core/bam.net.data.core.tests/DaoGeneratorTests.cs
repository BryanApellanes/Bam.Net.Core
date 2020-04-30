/*
	Copyright © Bryan Apellanes 2015  
*/

using System;
using System.CodeDom.Compiler;
using Bam.Net.Data.Schema;
using Bam.Net.Testing;

namespace Bam.Net.Data.Tests
{
    public class DaoGeneratorTests : CommandLineTestInterface
    {
        class TestRazorEngineDaoGenerator : DaoGenerator
        {
            public TestRazorEngineDaoGenerator() : base("Test") { }            
        }
                        
        private static void OutputCompilerErrors(CompilerResults results)
        {
            foreach (CompilerError error in results.Errors)
            {
                OutLineFormat("File=>{0}", ConsoleColor.Yellow, error.FileName);
                OutLineFormat("{0}, {1}::{2}", error.Line, error.Column, error.ErrorText);
            }
        }

        private static SchemaDefinition GetTestSchema()
        {
            SchemaManager mgr = new SchemaManager();
            SchemaDefinition schema = mgr.SetSchema("test");
            mgr.AddTable("monkey");
            return schema;
        }

    }
}
