namespace Bam.Net.Application
{
    public struct Runtime
    {
        public override bool Equals(object obj)
        {
            if (obj is Runtime rt)
            {
                if (string.IsNullOrEmpty(rt.Name))
                {
                    return string.IsNullOrEmpty(Name);
                }
                return rt.Name.Equals(Name);
            }

            return false;
        }

        public override string ToString()
        {
            return Name;
        }

        public static Runtime Current()
        {
            return For(OSInfo.Current);
        }

        public static Runtime For(string osName)
        {
            return For(osName.ToEnum<OSNames>());
        }
        
        public static Runtime For(OSNames osNames)
        {
            switch (osNames)
            {
                case OSNames.Linux:
                    return Linux;
                case OSNames.OSX:
                    return Mac;
                case OSNames.Invalid:
                case OSNames.Windows:
                default:
                    return Windows;
            }
        }
        
        public string Name { get; private set; }
        
        public static Runtime Windows
        {
            get { return new Runtime() {Name = "win10-x64"}; }
        }

        public static Runtime Linux
        {
            get { return new Runtime() {Name = "ubuntu.16.10-x64"}; }
        }
        
        public static Runtime Mac
        {
            get { return new Runtime() {Name = "osx-x64"}; }
        }
    }
}