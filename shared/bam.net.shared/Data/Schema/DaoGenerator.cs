/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.CodeDom.Compiler;
using Bam.Net.Logging;
using Microsoft.CSharp;
using Bam.Net.ServiceProxy;

namespace Bam.Net.Data.Schema
{
    /// <summary>
    /// A code generator that writes Dao code for a SchemaDefinition
    /// </summary>
    public partial class DaoGenerator
    {
        readonly List<Stream> _resultStreams = new List<Stream>();

        public DaoGenerator(string nameSpace)
            : this()
        {
            this.Namespace = nameSpace;
        }

        public DaoGenerator(string ns, Action<string> resultInspector)
            : this(ns)
        {
            this.RazorResultInspector = resultInspector;
        }

        public static List<string> DefaultReferenceAssemblies => new List<string>(AdHocCSharpCompiler.DefaultReferenceAssemblies);

        public IDaoCodeWriter DaoCodeWriter
        {
            get;
            set;
        }

        public IDaoTargetStreamResolver TargetStreamResolver
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether dispose will
        /// be called on the output streams after code generation.
        /// </summary>
        public bool DisposeOnComplete { get; set; }

        public bool GenerateQiClasses { get; set; }
        
        public Action<string> RazorResultInspector { get; set; }
        #region events
        
        /// <summary>
        /// The event that fires prior to code generation
        /// </summary>
        public event GeneratorEventDelegate GenerateStarted;

        /// <summary>
        /// The event that fires when code generation is complete
        /// </summary>
        public event GeneratorEventDelegate GenerateComplete;

        protected void OnGenerateStarted(SchemaDefinition schema)
        {
            GenerateStarted?.Invoke(this, schema);
        }

        protected void OnGenerateComplete(SchemaDefinition schema)
        {
            GenerateComplete?.Invoke(this, schema);
        }
        
        #endregion events

		/// <summary>
		/// If the generator compiled generated files, this will be the FileInfo 
		/// representing the compiled assembly
		/// </summary>
		public FileInfo DaoAssemblyFile { get; set; }

        public string Namespace { get; set; }
                 
        public void Generate(SchemaDefinition schema)
        {
            Generate(schema, "./");
        }

        /// <summary>
        /// Generate code for the specified schema
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="root"></param>
        public void Generate(SchemaDefinition schema, string root)
        {
            Generate(schema, null, root, null);
        }

        public void Generate(SchemaDefinition schema, string root, string partialsDir)
        {
            Generate(schema, null, root, partialsDir);
        }

        /// <summary>
        /// Generate code for the specified schema
        /// </summary>
        /// <param name="schema">The schema to generate code for</param>
        /// <param name="targetResolver">If specified, generated code will be 
        /// written to the stream returned by this function</param>
        /// <param name="root">The root file path to use if no target resolver is specified</param>
        public void Generate(SchemaDefinition schema, Func<string, Stream> targetResolver = null, string root = "./", string partialsDir = null)
        {
            if (string.IsNullOrEmpty(Namespace))
            {
                throw new NamespaceNotSpecifiedException();
            }
            DaoCodeWriter.Namespace = Namespace;

            OnGenerateStarted(schema);            
            
            DaoCodeWriter.WriteContextClass(schema, targetResolver, root);
            
            bool writePartial = !string.IsNullOrEmpty(partialsDir);
            if (writePartial)
            {
                EnsurePartialsDir(partialsDir);
            }

            foreach (Table table in schema.Tables)
            {
                if (writePartial)
                {
                    DaoCodeWriter.WritePartial(schema, targetResolver, root, table);
                }
                DaoCodeWriter.WriteDaoClass(schema, targetResolver, root, table);
                DaoCodeWriter.WriteQueryClass(schema, targetResolver, root, table);
                DaoCodeWriter.WritePagedQueryClass(schema, targetResolver, root, table);
                if (GenerateQiClasses)
                {
                    DaoCodeWriter.WriteQiClass(schema, targetResolver, root, table);
                }
                DaoCodeWriter.WriteCollectionClass(schema, targetResolver, root, table);
                DaoCodeWriter.WriteColumnsClass(schema, targetResolver, root, table);
            }

            OnGenerateComplete(schema);
        }
        
        private static void EnsurePartialsDir(string partialsDir)
        {
            DirectoryInfo partials = new DirectoryInfo(partialsDir);
            if (!partials.Exists)
            {
                partials.Create();
            }
        }

        public static Assembly[] GetReferenceAssemblies()
        {
            Assembly[] assembliesToReference = new Assembly[]{
                    typeof(DaoGenerator).Assembly,
                    typeof(ServiceProxySystem).Assembly,
                    typeof(DataTypes).Assembly,
                    typeof(Resolver).Assembly};
            return assembliesToReference;
        }
        
        protected virtual void WritePartialToStream(string code, Stream s)
        {
            WriteToStream(code, s);
        }
        
        private static void WriteToStream(string text, Stream s)
        {
            using (StreamWriter sw = new StreamWriter(s))
            {
                sw.Write(text);
                sw.Flush();
            }
        }

        private void SubscribeToEvents()
        {
            this.GenerateComplete += (g, schema) =>
            {
                if (DisposeOnComplete)
                {
                    foreach (Stream s in this._resultStreams)
                    {
                        s.Dispose();
                    }
                }

                this._resultStreams.Clear();
            };
        }
    }
}
