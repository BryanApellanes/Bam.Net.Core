/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.Common;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using Bam.Net.Data;
using Bam.Net;
using Bam.Net.Testing;
using System.Configuration;
using MySql.Data.MySqlClient;
using Bam.Net.Incubation;
using System.Data.OleDb;
using Bam.Net.CommandLine;
using Bam.Net.Data.Schema;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Bam.Net.Testing.Unit;

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
