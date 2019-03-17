namespace Bam.Net.Application
{
    public struct Runtime
    {
        public override bool Equals(object obj)
        {
            if (obj is Runtime rt)
            {
                return rt.Name.Equals(Name);
            }

            return false;
        }

        public override string ToString()
        {
            return Name;
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