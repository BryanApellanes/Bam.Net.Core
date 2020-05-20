/*
	Copyright © Bryan Apellanes 2015  
*/

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Bam.Net.CoreServices;
using Bam.Net.Data.Schema;

namespace Bam.Net.Data.Dynamic
{
    public partial class DaoAssemblyGenerator: IAssemblyGenerator
    {
        public DaoAssemblyGenerator(ISchemaExtractor schemaExtractor = null, string workspacePath = null)
        {
            this.ReferenceAssemblies = new Assembly[] { };
            this._referenceAssemblyPaths = new List<string>(AdHocCSharpCompiler.DefaultReferenceAssemblies);

            this.Workspace = workspacePath ?? $"./{nameof(DaoAssemblyGenerator)}";
            if (schemaExtractor != null)
            {
                SchemaExtractor = schemaExtractor;
            }
            else if (ServiceRegistry.Default.TryGet<ISchemaExtractor>(out ISchemaExtractor extractor))
            {
                SchemaExtractor = extractor;
            }
        }

        public DaoAssemblyGenerator(SchemaDefinition schema, SchemaNameMap nameMap, string workspacePath = null) : this()
        {
            this.Schema = schema;
            this.NameMap = nameMap;
        }

        public ISchemaExtractor SchemaExtractor { get; private set; }
        SchemaDefinition schema;
        readonly object schemaDefinitionLock = new object();
        public SchemaDefinition Schema
        {
            get
            {
                return schemaDefinitionLock.DoubleCheckLock(ref schema, () => SchemaExtractor.Extract());
            }
            set => schema = value;
        }
        
        public SchemaNameMap NameMap { get; set; }
        public string FileName { get; private set; }
        public string Workspace { get; set; }
        Assembly assembly;
        readonly object assemblyLock = new object();
        public Assembly Assembly
        {
            get
            {
                return assemblyLock.DoubleCheckLock(ref assembly, () => GetAssembly());
            }
        }

        public TargetTableEventDelegate OnTableStarted
        {
            get;
            set;
        }

        public GeneratorEventDelegate OnGenerateStarted
        {
            get;
            set;
        }

        public GeneratorEventDelegate OnGenerateComplete
        {
            get;
            set;
        }

        public string Namespace { get; set; }

        protected string FilePath { get; set; }

        protected Assembly[] ReferenceAssemblies { get; set; }

        readonly List<string> _referenceAssemblyPaths;
        protected string[] ReferenceAssemblyPaths
        {
            get
            {
                var results = new List<string>(ReferenceAssemblies.Select(a => a.GetFilePath()).ToArray());
                results.AddRange(_referenceAssemblyPaths);
                return results.ToArray();
            }
        }

        protected virtual SchemaDefinition GetSchemaDefinition()
        {
            if (NameMap != null)
            {
                MappedSchemaDefinition mappedDefinition = new MappedSchemaDefinition(Schema, NameMap);
                return mappedDefinition.MapSchemaClassAndPropertyNames();
            }
            else
            {
                return Schema;
            }
        }

        protected Assembly GetAssembly()
        {
            return GetAssemblyInfo().GetAssembly();
        }

        public GeneratedAssemblyInfo GetAssemblyInfo()
        {
            FileName = $"{Schema.Name}Dao";
            string fileName = Path.GetFileNameWithoutExtension(FileName);
            FilePath = Path.Combine(Workspace, $"{fileName}.dll");
            return GeneratedAssemblyInfo.GetGeneratedAssembly(FilePath, this);
        }

        readonly object _generateLock = new object();
        public GeneratedAssemblyInfo GenerateAssembly()
        {
            lock (_generateLock)
            {
                string sourcePath = Path.Combine(Workspace, "src");
                GenerateSource(sourcePath);
                GeneratedAssemblyInfo result = Compile(sourcePath);

                return result;
            }
        }

        public void WriteSource(string writeSourceTo)
        {
            GenerateSource(writeSourceTo);
        }

        public virtual void GenerateSource(string sourcePath)
        {
            DirectoryInfo sourceDir = new DirectoryInfo(sourcePath);
            if (!sourceDir.Exists)
            {
                sourceDir.Create();
            }
            DaoGenerator generator = new DaoGenerator(string.IsNullOrEmpty(Namespace) ? "{0}.Generated."._Format(this.GetType().Namespace).RandomLetters(6): Namespace);
            Subscribe(generator);
            generator.Generate(GetSchemaDefinition(), sourcePath);
        }

        public GeneratedAssemblyInfo Compile(string sourcePath, string fileName = null)
        {
            fileName = fileName ?? FileName;
            CompilerResults compileResult = AdHocCSharpCompiler.CompileDirectory(new DirectoryInfo(sourcePath), fileName, ReferenceAssemblyPaths, false);
            if (compileResult.Errors.Count > 0)
            {
                throw new CompilationException(compileResult);
            }

            GeneratedAssemblyInfo result = new GeneratedAssemblyInfo(FilePath, compileResult);
            result.Save();
            return result;
        }

        private void Subscribe(DaoGenerator generator)
        {
            if (OnGenerateStarted != null)
            {
                generator.GenerateStarted += OnGenerateStarted;
            }
            if (OnGenerateComplete != null)
            {
                generator.GenerateComplete += OnGenerateComplete;
            }
        }
    }
}
