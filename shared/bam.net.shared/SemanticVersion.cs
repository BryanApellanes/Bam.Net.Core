using System;
using System.Text;

namespace Bam.Net
{
    /// A standard version string in the following formats:
    /// If PreRelease is specified without Build
    /// {Major}.{Minor}.{Patch}-{PreRelease}
    ///
    /// If both PreRelease and Build are specified
    /// {Major}.{Minor}.{Patch}-{PreRelease}+{Build}
    ///
    /// If neither PreRelease nor Build are specified
    /// {Major}.{Minor}.{Patch}
    public class SemanticVersion
    {
        public SemanticVersion()
        {
            Major = 0;
            Minor = 0;
            Patch = 0;
            ReleasePrefix = "rc";
        }
        
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }
        
        public SemanticLifecycle Lifecycle { get; set; }
        public bool IsPreRelease { get; set; }
        /// <summary>
        /// A prerelease designation or blank.  Example values include
        /// "dev", "test" or "rc" (release candidate)
        /// </summary>
        public virtual string ReleasePrefix { get; set; }
        public virtual string Build { get; set; }

        public void ClearSuffix()
        {
            ReleasePrefix = string.Empty;
            Build = string.Empty;
        }

        public void SetSuffix(string suffix)
        {
            ClearSuffix();
            ReleasePrefix = suffix;
        }
        
        public override string ToString()
        {
            // TODO: account for SemanticLifecycleSpec value
            
            string version = $"{Major.ToString()}.{Minor.ToString()}.{Patch.ToString()}";
            string suffix = string.Empty;
            if (IsPreRelease)
            {
                suffix = !string.IsNullOrEmpty(Build) ? $"-{ReleasePrefix}+{Build}" : $"-{ReleasePrefix}";
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
                    ReleasePrefix = componentValue;
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
                value.ReleasePrefix = suffixParts[0];
                if (suffixParts.Length == 2)
                {
                    value.Build = suffixParts[1];
                }
            }

            return value;
        }
    }
}