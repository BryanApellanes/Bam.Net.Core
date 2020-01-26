using System;
using System.Text;
using Bam.Net.Analytics;
using Microsoft.CodeAnalysis.Emit;

namespace Bam.Net
{
    /// A standard version string in the following formats:
    /// If PreReleasePrefix is specified without Build
    /// {Major}.{Minor}.{Patch}-{PreReleasePrefix}
    ///
    /// If Build is specified without PreReleasePrefix
    /// {Major}.{Minor{.{Patch}-{Build}
    /// If both PreReleasePrefix and Build are specified
    /// {Major}.{Minor}.{Patch}-{PreReleasePrefix}+{Build}
    ///
    /// If neither PreReleasePrefix nor Build are specified
    /// {Major}.{Minor}.{Patch}
    public class SemanticVersion
    {
        public SemanticVersion()
        {
            Major = 1;
            Minor = 0;
            Patch = 0;
            PreReleasePrefix = string.Empty;
        }
        
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }
        
        public SemanticLifecycle Lifecycle { get; set; }
        /// <summary>
        /// A prerelease designation or blank.  Example values include
        /// "dev", "test" or "rc" (release candidate)
        /// </summary>
        public virtual string PreReleasePrefix { get; set; }
        
        /// <summary>
        /// A value used to track the build number, commonly the git commit.
        /// </summary>
        public virtual string Build { get; set; }

        public void ClearSuffix()
        {
            PreReleasePrefix = string.Empty;
            Build = string.Empty;
        }

        public void SetSuffix(string suffix)
        {
            ClearSuffix();
            PreReleasePrefix = suffix;
        }
        
        public override string ToString()
        {
            if (Lifecycle != SemanticLifecycle.None)
            {
                switch (Lifecycle)
                {
                    case SemanticLifecycle.Dev:
                        PreReleasePrefix = "dev";
                        break;
                    case SemanticLifecycle.Test:
                        PreReleasePrefix = "test";
                        break;
                    case SemanticLifecycle.Staging:
                        PreReleasePrefix = "staging";
                        break;
                    case SemanticLifecycle.Release:
                        PreReleasePrefix = string.Empty;
                        break;
                }
            }
            
            string version = $"{Major.ToString()}.{Minor.ToString()}.{Patch.ToString()}";
            string suffix = string.Empty;
            if (!string.IsNullOrEmpty(PreReleasePrefix))
            {
                suffix = !string.IsNullOrEmpty(Build) ? $"-{PreReleasePrefix}+{Build}" : $"-{PreReleasePrefix}";
            }
            else if (!string.IsNullOrEmpty(Build))
            {
                suffix = $"-{Build}";
            }

            return $"{version}{suffix}";
        }

        public SemanticVersion Next(VersionSpec spec = VersionSpec.Patch, string componentValue = "")
        {
            SemanticVersion next = this.CopyAs<SemanticVersion>();
            return next.Increment(spec, componentValue);
        }
        
        public SemanticVersion Increment(VersionSpec spec = VersionSpec.Patch, string componentValue = "")
        {
            switch (spec)
            {
                case VersionSpec.Major:
                    ++Major;
                    break;
                case VersionSpec.Minor:
                    ++Minor;
                    break;
                case VersionSpec.Patch:
                    ++Patch;
                    break;
                case VersionSpec.PreRelease:
                    PreReleasePrefix = componentValue;
                    break;
                case VersionSpec.Build:
                    Build = componentValue;
                    break;
            }

            return this;
        }

        public static bool TryParse(string version, out SemanticVersion semanticVersion)
        {
            semanticVersion = null;
            try
            {
                semanticVersion = Parse(version);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
        public static SemanticVersion Parse(string version)
        {
            string[] versionAndExtra = version.DelimitSplit("-");
            Args.ThrowIf(versionAndExtra.Length > 2, "Unrecognized SemanticVersion: {0}", version);
            string versionPart = versionAndExtra[0];
            string suffix = string.Empty;
            if (versionAndExtra.Length == 2)
            {
                suffix = versionAndExtra[1];
            }

            string[] versionParts = versionPart.DelimitSplit(".");
            
            Args.ThrowIf(versionParts.Length != 3, "Unrecognized SemanticVersion: {0}", version);
            SemanticVersion value = new SemanticVersion {Major = int.Parse(versionParts[0]), Minor = int.Parse(versionParts[1]), Patch = int.Parse(versionParts[2])};

            if (!string.IsNullOrEmpty(suffix))
            {
                string[] suffixParts = suffix.DelimitSplit("-", "+");
                Args.ThrowIf(suffixParts.Length > 2, "Unrecognized SemanticVersion: {0}", version);
                value.PreReleasePrefix = suffixParts[0];
                if (suffixParts.Length == 2)
                {
                    value.Build = suffixParts[1];
                }
            }

            return value;
        }

        public override bool Equals(object obj)
        {
            if (obj is SemanticVersion version)
            {
                return ToString().Equals(version.ToString());
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static bool operator >(SemanticVersion one, SemanticVersion two)
        {
            return !(one < two);
        }
        public static bool operator <(SemanticVersion one, SemanticVersion two)
        {
            if (one.Major < two.Major)
            {
                return true;
            }

            if (one.Major == two.Major && one.Minor < two.Minor)
            {
                return true;
            }

            return one.Major == two.Major && one.Minor == two.Minor && one.Patch < two.Patch;
        }

        public static bool operator >=(SemanticVersion one, SemanticVersion two)
        {
            if (one > two)
            {
                return true;
            }

            return one.Major == two.Major && one.Minor == two.Minor && one.Patch == two.Patch;
        }

        public static bool operator <=(SemanticVersion one, SemanticVersion two)
        {
            if (one < two)
            {
                return true;
            }
            
            return one.Major == two.Major && one.Minor == two.Minor && one.Patch == two.Patch;
        }
    }
}