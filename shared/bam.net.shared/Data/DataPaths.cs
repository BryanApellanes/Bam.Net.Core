using Bam.Net.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.Data
{
    public class DataPaths
    {
        public static DataPaths Get(IDataDirectoryProvider dataDirectoryProvider, IApplicationNameProvider applicationNameProvider = null)
        {
            applicationNameProvider = applicationNameProvider ?? DefaultConfigurationApplicationNameProvider.Instance;
            return new DataPaths
            {
                DataRoot = dataDirectoryProvider.GetRootDataDirectory().FullName,
                SysData = dataDirectoryProvider.GetSysDataDirectory().FullName,

                AppData = dataDirectoryProvider.GetAppDataDirectory(applicationNameProvider).FullName,
                UserData = dataDirectoryProvider.GetAppUsersDirectory(applicationNameProvider).FullName,
                AppDatabase = dataDirectoryProvider.GetAppDatabaseDirectory(applicationNameProvider).FullName,
                AppRepository = dataDirectoryProvider.GetAppRepositoryDirectory(applicationNameProvider).FullName,
                AppFiles = dataDirectoryProvider.GetAppFilesDirectory(applicationNameProvider).FullName,
                AppEmailTemplates = dataDirectoryProvider.GetAppEmailTemplatesDirectory(applicationNameProvider).FullName
            };
        }

        public string DataRoot { get; set; }
        public string SysData { get; set; }
        public string AppData { get; set; }
        public string UserData { get; set; }
        public string AppDatabase { get; set; }
        public string AppRepository { get; set; }
        public string AppFiles { get; set; }
        public string AppEmailTemplates { get; set; }
    }
}
