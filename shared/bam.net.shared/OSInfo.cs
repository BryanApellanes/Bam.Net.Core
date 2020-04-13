using Bam.Net.CommandLine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Bam.Net.Logging;

namespace Bam.Net
{
    public class OSInfo
    {
        public const string DefaultCoreVersion = "2.2.4";
        public const string DefaultLibSubfolder = "netcoreapp2.2";
        
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

        public static bool IsUnix => Current != OSNames.Windows;
        public static bool IsWindows => Current == OSNames.Windows;

        public static string CoreVersion => Config.Current["CoreVersion"].Or(DefaultCoreVersion);

        private static readonly Dictionary<OSNames, string> _referenceRuntimeNames = new Dictionary<OSNames, string>
        {
            {OSNames.Invalid, "win-x64"},
            {OSNames.Windows, "win-x64"},
            {OSNames.Linux, "linux-x64"},
            {OSNames.OSX, "osx-x64"}
        };

        public static string CurrentRuntime => BuildRuntimeName;

        public static string ReferenceRuntime => _referenceRuntimeNames[Current];

        static readonly Dictionary<OSNames, string> _buildRuntimeNames = new Dictionary<OSNames, string>
        {
            {OSNames.Invalid, "ubuntu.16.10-x64"},
            {OSNames.Windows, "win10-x64"},
            {OSNames.Linux, "ubuntu.16.10-x64"},
            {OSNames.OSX, "osx-x64"}
        };
        
        public static string BuildRuntimeName => _buildRuntimeNames[Current];

        public static string DefaultToolPath(string fileName)
        {
            return $"/opt/bam/tools/{fileName}";
        }
        
        public static bool TryGetPath(string fileName, out string path)
        {
            try
            {
                path = GetPath(fileName);
                Log.Info("Found file {0}: {1}", fileName, path);
                return true;
            }
            catch (Exception ex)
            {
                Log.Warn("Exception finding path for {0}: {1}", fileName, ex.Message);
                path = string.Empty;
                return false;
            }
        }
        
        public static string GetPath(string fileName)
        {
            if (Current == OSNames.Windows)
            {
                ProcessOutput whereOutput = $"where {fileName}".Run();
                return ResolvePath(fileName, whereOutput.StandardOutput);
            }
            ProcessOutput whichOutput = $"which {fileName}".Run();
            return ResolvePath(fileName, whichOutput.StandardOutput);
        }

        private static string ResolvePath(string fileName, string output)
        {
            string[] lines = output.DelimitSplit("\r", "\n");
            if (lines.Length == 2)
            {
                return lines[1];
            }
            if (lines.Length == 0 || lines.Length > 2)
            {
                Args.Throw<ArgumentException>("Unable to resolve path for {0}: \r\n\t{1}", fileName, output);
            }
            return lines[0];
        }
    }
}
