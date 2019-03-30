using Bam.Net;

namespace bam.Shell
{
    public class ShellSettings
    {
        public ShellSettings()
        {
            if (OSInfo.Current == OSNames.Windows)
            {
                Editor = OSInfo.GetPath("notepad");
            }
            else
            {
                Editor = OSInfo.GetPath("vi");
            }
        }
        public string Editor { get; set; }

        static ShellSettings _current;
        static object _currentLock = new object();
        public static ShellSettings Current
        {
            get { return _currentLock.DoubleCheckLock(ref _current, () => new ShellSettings()); }
        }
    }
}