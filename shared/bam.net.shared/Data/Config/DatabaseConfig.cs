using System;
using System.Collections.Generic;

namespace Bam.Net.Data
{
    public class DatabaseConfig
    {
        public string ConnectionString { get; set; }
        
        public RelationalDatabaseTypes DatabaseType { get; set; }

        private Dictionary<RelationalDatabaseTypes, Type> _databaseTypes;

        public object GetDatabase()
        {
            throw new NotImplementedException();
        }

        public T GetDatabase<T>() where T : Database
        {
            return (T) GetDatabase();
        }
    }
}