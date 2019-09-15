using System;
using System.Diagnostics;
using System.IO;
using Bam.Net.Logging;

namespace Bam.Net.CommandLine
{
    public class PidFile
    {
        public PidFile()
        {
            ProcessId = -1;
            Process = Process.GetCurrentProcess();
            Init();
        }

        public PidFile(Process process)
        {
            ProcessId = -1;
            Process = process;
            Init();
        }

        public PidFile(Process process, string commandLineArgs)
        {
            ProcessId = -1;
            Process = process;
            Init(commandLineArgs);
        }

        public PidFile(int processId, string commandLineArgs)
        {
            ProcessId = processId;
            commandLineArgs = commandLineArgs;
        }

        public void Save()
        {
            Save(PidFilePath);
        }

        public void Save(string path)
        {
            PidFilePath = path;
            $"{ProcessId}~{CommandLineArgs}".SafeWriteToFile(PidFilePath, true);
        }

        public bool Kill(bool matchCommandLineArgs = true, ILogger logger = null)
        {
            if (ProcessId != -1)
            {
                try
                {
                    Process = System.Diagnostics.Process.GetProcessById(ProcessId);
                }
                catch (Exception ex)
                {
                    logger.Error("Failed to load process by id ({0}): ({1})", ProcessId, ex.Message);
                }
            }
            if (Process != null)
            {
                try
                {
                    if (matchCommandLineArgs)
                    {
                        if(Process.StartInfo.Arguments.Equals(CommandLineArgs, StringComparison.InvariantCultureIgnoreCase))
                        {
                            logger.Warning("CommandLineArgs match ({0}): killing old process", CommandLineArgs);
                            Process.Kill();
                        }
                    }
                    else
                    {
                        logger.Warning("Killing old process ({0})", ProcessId);
                        Process.Kill();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    logger = logger ?? Log.Default;
                    logger.Error("Exception killing process: ({0})", ex.Message);
                    return false;
                }
            }

            return false;
        }
        
        private static PidFile _current;
        static readonly object _pidFileLock = new object();
        public static PidFile Current
        {
            get { return _pidFileLock.DoubleCheckLock(ref _current, () => new PidFile()); }
        }

        public static PidFile Load(string path, ILogger logger = null)
        {
            string fileContent = path.SafeReadFile();
            string pidString = fileContent.ReadUntil('~', out string fileCommandLineArgs);

            Args.ThrowIf<InvalidOperationException>(!int.TryParse(pidString, out int pid), "Unable to parse pid from file ({0})", path);

            try
            {
                Process process = System.Diagnostics.Process.GetProcessById(pid);
                return new PidFile(process, fileCommandLineArgs);
            }
            catch (Exception ex)
            {
                logger = logger ?? Log.Default;
                logger.Error("Exception loading process id specified in PidFile ({0}): {1}", path, ex.Message);
            }
            return new PidFile(-1, string.Empty);
        }
        
        protected Process Process { get; set; }
        protected FileInfo MainModule { get; set; } 
        public int ProcessId { get; set; }
        public string CommandLineArgs { get; set; }
        public string Name { get; set; }
        public string PidFilePath { get; set; }

        private void Init()
        {
            Init(string.Join(" ", Environment.GetCommandLineArgs()));
        }
        
        private void Init(string commandLineArgs)
        {
            ProcessId = Process.Id;
            MainModule = new FileInfo(Process.MainModule.FileName);
            Name = $"{System.IO.Path.GetFileNameWithoutExtension(MainModule.Name)}.pid";
            PidFilePath = System.IO.Path.Combine(MainModule.Directory.FullName, Name);
            CommandLineArgs = commandLineArgs;
        }
    }
}