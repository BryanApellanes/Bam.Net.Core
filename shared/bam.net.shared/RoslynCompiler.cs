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
using Bam.Net.Logging;
using CsQuery.ExtensionMethods;

namespace Bam.Net
{
    public class RoslynCompiler : ICompiler
    {
        public RoslynCompiler()
        {
            _referencePaths = new HashSet<string>();
            _referenceAssemblies = new HashSet<Assembly>();
            OutputKind = OutputKind.DynamicallyLinkedLibrary;
            ReferenceAssemblies = DefaultReferenceAssemblies;
        }

        HashSet<Assembly> _referenceAssemblies;
        public Assembly[] ReferenceAssemblies
        {
            get { return _referenceAssemblies.ToArray(); }
            set
            {
                _referenceAssemblies.Clear();
                value.ForEach(a => _referenceAssemblies.Add(a));
            }
        }

        HashSet<string> _referencePaths;
        public string[] ReferencePaths
        {
            get { return _referencePaths.ToArray(); }
        }
        
        public OutputKind OutputKind { get; set; }

        public RoslynCompiler AddAssemblyReference(Type type)
        {
            return AddAssemblyReference(type.Assembly);
        }

        public RoslynCompiler AddAssemblyReference(Assembly assembly)
        {
            _referenceAssemblies.Add(assembly);
            return this;
        }
        
        public RoslynCompiler AddAssemblyReference(string path)
        {
            _referencePaths.Add(path);
            return this;
        }
        
        public RoslynCompiler AddAssemblyReference(FileInfo assemblyFile)
        {
            _referenceAssemblies.Add(Assembly.Load(assemblyFile.FullName));
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
            return Compile(assemblyFileName,
                sourceFiles.Select(f => SyntaxFactory.ParseSyntaxTree(f.ReadAllText())).ToArray());
        }
        
        public byte[] Compile(string assemblyName, string sourceCode)
        {
            SyntaxTree tree = SyntaxFactory.ParseSyntaxTree(sourceCode);
            return Compile(assemblyName, tree);
        }

        public byte[] Compile(string assemblyName, params SyntaxTree[] syntaxTrees)
        {
            return Compile(assemblyName, GetMetadataReferences, syntaxTrees);
        }

        public byte[] Compile(string assemblyName, Func<MetadataReference[]> getMetaDataReferences, params SyntaxTree[] syntaxTrees)
        {
            getMetaDataReferences = getMetaDataReferences ?? GetMetadataReferences;
            CSharpCompilation compilation = CSharpCompilation.Create(assemblyName)
                .WithOptions(new CSharpCompilationOptions(this.OutputKind))
                .AddReferences(getMetaDataReferences())
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

        static Assembly[] _defaultReferenceAssemblies = new Assembly[] { };
        public static Assembly[] DefaultReferenceAssemblies
        {
            get
            {
                if (_defaultReferenceAssemblies.Length == 0)
                {
                    List<Assembly> defaultAssemblies = new List<Assembly>
                    {
                        typeof(System.Dynamic.DynamicObject).Assembly,
                        typeof(System.Xml.XmlDocument).Assembly,
                        typeof(System.Data.DataTable).Assembly,
                        typeof(object).Assembly,
                        Assembly.GetExecutingAssembly()
                    };
                    AddCommonReferenceAssemblies(defaultAssemblies);
                    _defaultReferenceAssemblies = defaultAssemblies.ToArray();
                }
                
                return _defaultReferenceAssemblies;
            }
        }

        private static void AddCommonReferenceAssemblies(List<Assembly> defaultAssemblies)
        {
            RuntimeConfig config = RuntimeSettings.GetConfig();
            DirectoryInfo commonAssemblies = new DirectoryInfo(config.ReferenceAssembliesDir);
            if (commonAssemblies.Exists)
            {
                foreach (FileInfo file in commonAssemblies.GetFiles())
                {
                    if (file.HasExtension(".dll") || file.HasExtension(".exe"))
                    {
                        try
                        {
                            defaultAssemblies.Add(Assembly.LoadFile(file.FullName));
                        }
                        catch (Exception ex)
                        {
                            Log.Warn("Exception loading reference assembly {0}: {1}", file.FullName, ex.Message);
                        }
                    }
                }
            }
        }

        private MetadataReference[] GetMetadataReferences()
        {
            List<MetadataReference> metadataReferences = new List<MetadataReference>();
            metadataReferences.AddRange(ReferencePaths.Select(p => MetadataReference.CreateFromFile(p)));
            metadataReferences.AddRange(ReferenceAssemblies.Select(ass => MetadataReference.CreateFromFile(ass.Location)).ToArray());
            return metadataReferences.ToArray();
        }
    }
}
