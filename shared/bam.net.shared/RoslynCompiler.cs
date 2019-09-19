using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Text;
using Bam.Net.CoreServices.AssemblyManagement;
using Bam.Net.Logging;
using CsQuery.ExtensionMethods;
using GraphQL;

namespace Bam.Net
{
    public class RoslynCompiler : ICompiler
    {
        public RoslynCompiler()
        {
            _referenceAssemblyPaths = new HashSet<string>();
            _assembliesToReference = new HashSet<Assembly>();
            OutputKind = OutputKind.DynamicallyLinkedLibrary;
            AssembliesToReference = DefaultAssembliesToReference;
            ReferenceAssemblyResolver = ReferenceAssemblyResolver ?? Bam.Net.CoreServices.AssemblyManagement.ReferenceAssemblyResolver.Current;
        }

        public RoslynCompiler(IReferenceAssemblyResolver referenceAssemblyResolver) : this()
        {
            ReferenceAssemblyResolver = referenceAssemblyResolver;
        }
        
        public IReferenceAssemblyResolver ReferenceAssemblyResolver { get; set; }

        readonly HashSet<Assembly> _assembliesToReference;
        public Assembly[] AssembliesToReference
        {
            get => _assembliesToReference.ToArray();
            set
            {
                _assembliesToReference.Clear();
                value.ForEach(a => _assembliesToReference.Add(a));
            }
        }

        HashSet<string> _referenceAssemblyPaths;
        public string[] ReferenceAssemblyPaths
        {
            get { return _referenceAssemblyPaths.ToArray(); }
        }
        
        public OutputKind OutputKind { get; set; }

        public RoslynCompiler AddAssemblyReference(Type type)
        {
            return AddAssemblyReference(type.Assembly);
        }

        public RoslynCompiler AddAssemblyReference(Assembly assembly)
        {
            _assembliesToReference.Add(assembly);
            return this;
        }

        public RoslynCompiler AddResolvedAssemblyReference(string assemblyName)
        {
            return AddAssemblyReference(ReferenceAssemblyResolver.ResolveReferenceAssemblyPath(assemblyName));
        } 
        
        public RoslynCompiler AddAssemblyReference(string path)
        {
            _referenceAssemblyPaths.Add(path);
            return this;
        }
        
        public RoslynCompiler AddAssemblyReference(FileInfo assemblyFile)
        {
            _assembliesToReference.Add(Assembly.Load(assemblyFile.FullName));
            return this;
        }

        public Assembly CompileAssembly(string assemblyFileName, DirectoryInfo directoryInfo)
        {
            return CompileAssembly(assemblyFileName, directoryInfo.GetFiles("*.cs").ToArray());
        }

        public Assembly CompileAssembly(string assemblyFileName, params FileInfo[] sourceFiles)
        {
            return Assembly.Load(Compile(assemblyFileName, sourceFiles));
        }

        public byte[] Compile(string assemblyFileName, DirectoryInfo directoryInfo)
        {
            return Compile(assemblyFileName, directoryInfo.GetFiles("*.cs").ToArray());
        }
        
        public byte[] Compile(string assemblyFileName, params FileInfo[] sourceFiles)
        {
            return Compile(assemblyFileName, sourceFiles.Select(f => SyntaxFactory.ParseSyntaxTree(f.ReadAllText())).ToArray());
        }

        public Assembly CompileAssembly(string assemblyName, string sourceCode)
        {
            return Assembly.Load(Compile(assemblyName, sourceCode));
        }
        
        public byte[] Compile(string assemblyName, string sourceCode)
        {
            SyntaxTree tree = SyntaxFactory.ParseSyntaxTree(sourceCode);
            return Compile(assemblyName, tree);
        }

        public byte[] Compile(string assemblyName, params SyntaxTree[] syntaxTrees)
        {
            return Compile(assemblyName, GetMetaDataReferences, syntaxTrees);
        }

        public byte[] Compile(string assemblyName, Func<MetadataReference[]> getMetaDataReferences, params SyntaxTree[] syntaxTrees)
        {
            getMetaDataReferences = getMetaDataReferences ?? GetMetaDataReferences;
            MetadataReference[] metaDataReferences = getMetaDataReferences();
            CSharpCompilation compilation = CSharpCompilation.Create(assemblyName)
                .WithOptions(new CSharpCompilationOptions(this.OutputKind))
                .AddReferences(metaDataReferences)
                .AddSyntaxTrees(syntaxTrees);
            
            using(MemoryStream stream = new MemoryStream())
            {
                EmitResult compileResult = compilation.Emit(stream);
                if (!compileResult.Success)
                {
                    throw new RoslynCompilationException(compileResult.Diagnostics);
                }
                return stream.GetBuffer();
            }
        }

        static Assembly[] _defaultAssembliesToReference = new Assembly[] { };
        public static Assembly[] DefaultAssembliesToReference
        {
            get
            {
                if (_defaultAssembliesToReference.Length == 0)
                {
                    List<Assembly> defaultAssemblies = new List<Assembly>
                    {
                        typeof(System.Dynamic.DynamicObject).Assembly,
                        typeof(System.Xml.XmlDocument).Assembly,
                        typeof(System.Data.DataTable).Assembly,
                        typeof(object).Assembly,
                        typeof(Newtonsoft.Json.JsonWriter).Assembly,
                        typeof(FileInfo).Assembly,
                        typeof(System.Linq.Enumerable).Assembly,
                        Assembly.GetExecutingAssembly()
                    };
                    _defaultAssembliesToReference = defaultAssemblies.ToArray();
                }
                
                return _defaultAssembliesToReference;
            }
        }

        private MetadataReference[] GetMetaDataReferences()
        {
            List<MetadataReference> metadataReferences = new List<MetadataReference>();
            IReferenceAssemblyResolver referenceAssemblyResolver = ReferenceAssemblyResolver ?? Bam.Net.CoreServices.AssemblyManagement.ReferenceAssemblyResolver.Current;

            if (OSInfo.Current == OSNames.Windows)
            {
                metadataReferences.Add(MetadataReference.CreateFromFile(RuntimeSettings.GetMsCoreLibPath()));
            }

            metadataReferences.Add(MetadataReference.CreateFromFile(referenceAssemblyResolver.ResolveSystemRuntimePath()));
            metadataReferences.Add(MetadataReference.CreateFromFile(referenceAssemblyResolver.ResolveNetStandardPath()));
            metadataReferences.Add(MetadataReference.CreateFromFile(RuntimeSettings.GetBamAssemblyPath()));
            metadataReferences.AddRange(ReferenceAssemblyPaths.Select(p => MetadataReference.CreateFromFile(p)));
            metadataReferences.AddRange(AssembliesToReference.Select(ass => MetadataReference.CreateFromFile(ass.Location)).ToArray());
            return metadataReferences.ToArray();
        }
    }
}
