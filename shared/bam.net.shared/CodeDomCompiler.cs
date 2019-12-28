using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Bam.Net
{
    [Obsolete("CodeDomCompiler is obsolete, use RoslynCompiler instead")]
    public class CodeDomCompiler : ICompiler
    {
        public CodeDomCompiler(Assembly[] referenceAssemblies)
        {
            AssembliesToReference = referenceAssemblies;
        }

        public Assembly[] AssembliesToReference { get; set; }

        public Assembly CompileAssembly(string assemblyFileName, DirectoryInfo directoryInfo)
        {
            CompilerResults results = AdHocCSharpCompiler.CompileDirectory(directoryInfo, assemblyFileName, AssembliesToReference, Path.GetExtension(assemblyFileName).EndsWith("exe"));
            ThrowIfErrors(results);
            return results.CompiledAssembly;
        }

        public Assembly CompileAssembly(string assemblyFileName, FileInfo[] files)
        {
            CompilerResults results = AdHocCSharpCompiler.CompileFiles(files, assemblyFileName, AssembliesToReference, Path.GetExtension(assemblyFileName).EndsWith("exe"));
            ThrowIfErrors(results);
            return results.CompiledAssembly;
        }

        public byte[] Compile(string assemblyFileName, params FileInfo[] sourceFiles)
        {
            Assembly assembly = CompileAssembly(assemblyFileName, sourceFiles);
            return File.ReadAllBytes(assembly.GetFilePath());
        }
        
        public byte[] Compile(string assemblyFileName, string sourceCode)
        {
            CompilerResults results = AdHocCSharpCompiler.CompileSource(sourceCode, assemblyFileName, AssembliesToReference, Path.GetExtension(assemblyFileName).EndsWith("exe"));
            ThrowIfErrors(results);
            return File.ReadAllBytes(results.CompiledAssembly.GetFilePath());
        }

        private static void ThrowIfErrors(CompilerResults results)
        {
            if (results.Errors != null &&
                results.Errors.Count > 0)
            {
                throw new CompilationException(results);
            }
        }
    }
}
