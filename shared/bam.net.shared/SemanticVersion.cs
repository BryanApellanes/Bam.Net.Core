using System;
using System.Text;

namespace Bam.Net
{
    public class SemanticVersion
    {
        public SemanticVersion()
        {
            Major = 0;
            Minor = 0;
            Patch = 0;
        }
        
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }
        public string PreRelease { get; set; }
        public string Build { get; set; }

        public override string ToString()
        {
            string version = $"{Major.ToString()}.{Minor.ToString()}.{Patch.ToString()}";
            string suffix = string.Empty;
            if (!string.IsNullOrEmpty(PreRelease))
            {
                suffix = !string.IsNullOrEmpty(Build) ? $"-{PreRelease}+{Build}" : $"-{PreRelease}";
            }
            else if (!string.IsNullOrEmpty(Build))
            {
                suffix = $"-rc+{Build}";
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
                    PreRelease = componentValue;
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
                value.PreRelease = suffixParts[0];
                if (suffixParts.Length == 2)
                {
                    value.Build = suffixParts[1];
                }
            }

            return value;
        }
    }
}