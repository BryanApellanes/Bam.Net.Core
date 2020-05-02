using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.Data
{
    /// <summary>
    /// Class used to report diagnostic information about a database.
    /// </summary>
    public class DatabaseInfo
    {
        public DatabaseInfo(Database database)
        {
            Args.ThrowIfNull(database, "database");
            Database = database;
        }
        
        protected Database Database { get; }

        public string DatabaseType => Database.GetType().FullName;
        public string ConnectionString => Database.ConnectionString;
        public string ConnectionName => Database.ConnectionName;

        public override string ToString()
        {
            return $"{DatabaseType}:{ConnectionName}";
        }

        public override int GetHashCode()
        {
            return $"{DatabaseType}{ConnectionString}{ConnectionName}".GetHashCode();
        }

        public override bool Equals(object obj)
        {
            DatabaseInfo dbInfo = obj as DatabaseInfo;
            if (dbInfo == null)
            {
                return false;
            }

            
            return dbInfo.DatabaseType.Or("").Equals(this.DatabaseType) &&
                   dbInfo.ConnectionString.Or("").Equals(this.ConnectionString) &&
                   dbInfo.ConnectionName.Or("").Equals(this.ConnectionName);
        }
    }
}
