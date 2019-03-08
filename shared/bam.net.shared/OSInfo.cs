using Bam.Net.CommandLine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Bam.Net
{
    public class OSInfo
    {
        static OSNames _current;
        public static OSNames Current
        {
            get
            {
                if (_current == OSNames.Invalid)
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        _current = OSNames.Windows;
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        _current = OSNames.Linux;
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        _current = OSNames.OSX;
                    }
                }

                return _current;
            }
        }

        public static string GetPath(string fileName)
        {
            if (Current == OSNames.Windows)
            {
                ProcessOutput whereOutput = $"where {fileName}".Run();
                return ResolvePath(whereOutput.StandardOutput);
            }
            ProcessOutput whichOutput = $"which {fileName}".Run();
            return ResolvePath(whichOutput.StandardOutput);
        }

        private static string ResolvePath(string output)
        {
            string[] lines = output.DelimitSplit("\r", "\n");
            if (lines.Length == 2)
            {
                return lines[1];
            }
            if (lines.Length == 0 || lines.Length > 2)
            {
                Args.Throw<ArgumentException>("Unable to resolve path for {0}", output);
            }
            return lines[0];
        }
    }
}
