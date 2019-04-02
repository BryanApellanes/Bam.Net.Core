using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Bam.Net
{
    public interface ICompiler
    {
        Assembly[] ReferenceAssemblies { get; set; }

        Assembly Compile(string assemblyFileName, DirectoryInfo directoryInfo);
        Assembly Compile(string assemblyFileName, FileInfo[] files);
        byte[] Compile(string assemblyFileName, string sourceCode);        
    }
}
