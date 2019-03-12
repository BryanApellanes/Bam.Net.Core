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
        public static DataPaths Get(IDataProvider dataProvider)
        {
            return new DataPaths
            {
                DataRoot = dataProvider.GetRootDataDirectory().FullName,
                SysData = dataProvider.GetSysDataDirectory().FullName,

                AppData = dataProvider.GetAppDataDirectory(DefaultConfigurationApplicationNameProvider.Instance).FullName,
                UserData = dataProvider.GetAppUsersDirectory(DefaultConfigurationApplicationNameProvider.Instance).FullName,
                AppDatabase = dataProvider.GetAppDatabaseDirectory(DefaultConfigurationApplicationNameProvider.Instance).FullName,
                AppRepository = dataProvider.GetAppRepositoryDirectory(DefaultConfigurationApplicationNameProvider.Instance).FullName,
                AppFiles = dataProvider.GetAppFilesDirectory(DefaultConfigurationApplicationNameProvider.Instance).FullName,
                AppEmailTemplates = dataProvider.GetAppEmailTemplatesDirectory(DefaultConfigurationApplicationNameProvider.Instance).FullName
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
