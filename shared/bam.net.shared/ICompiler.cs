using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Bam.Net
{
    public interface ICompiler
    {
        Assembly[] AssembliesToReference { get; set; }

        Assembly CompileAssembly(string assemblyFileName, DirectoryInfo directoryInfo);
        Assembly CompileAssembly(string assemblyFileName, FileInfo[] files);
        byte[] Compile(string assemblyFileName, params FileInfo[] sourceFiles);
        byte[] Compile(string assemblyFileName, string sourceCode);        
    }
}
