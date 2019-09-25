using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bam.Net
{
    public class RoslynCompilationException: Exception
    {
        public RoslynCompilationException(IEnumerable<Diagnostic> diagnostics) : base($"Compilation exception::\r\n{GetMessage(diagnostics)}")
        {
            Diagnostics = diagnostics.ToArray();
        }

        public Diagnostic[] Diagnostics { get; }
        
        internal static string GetMessage(IEnumerable<Diagnostic> diagnostics)
        {
            StringBuilder builder = new StringBuilder();
            foreach(Diagnostic diagnostic in diagnostics)
            {
                builder.AppendLine(GetMessage(diagnostic));
            }
            return builder.ToString();
        }

        internal static string GetMessage(Diagnostic diagnostic)
        {
            return diagnostic.ToString();
        }
        
    }
}
