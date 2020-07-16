using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bam.Net.ServiceProxy.Secure;
using CsQuery.ExtensionMethods;

namespace Bam.Net.Data.Schema
{
    public class ReverseDaoSchemaExtractor : SchemaExtractor
    {
        public ReverseDaoSchemaExtractor(Assembly assembly, string nameSpace)
        {
            Assembly = assembly;
            Namespace = nameSpace;
            DataTypeTranslator = new DataTypeTranslator();
        }
        
        public Assembly Assembly { get; set; }
        public string Namespace { get; set; }

        private Dictionary<string, Type> _daoTypes;
        private Dictionary<string, List<ColumnAttribute>> _columnAttributes;
        protected DataTypeTranslator DataTypeTranslator { get; }

        private bool _analyzed;
        private readonly object _analyzeLock = new object();
        public void Analyze()
        {
            if (!_analyzed)
            {
                lock (_analyzeLock)
                {
                    if (_daoTypes == null)
                    {
                        Args.ThrowIfNull(Assembly, nameof(Assembly));
                        Args.ThrowIfNullOrEmpty(Namespace, nameof(Namespace));

                        _daoTypes = new Dictionary<string, Type>();
                
                        Assembly
                            .GetTypes()
                            .Where(type =>
                                type.Namespace != null && 
                                type.Namespace.Equals(Namespace) &&
                                type.ExtendsType(typeof(Dao)) &&
                                type.HasCustomAttributeOfType<TableAttribute>())
                            .Each(type => _daoTypes.Add(Dao.TableName(type), type));
                    }

                    if (_columnAttributes == null)
                    {
                        _columnAttributes = new Dictionary<string, List<ColumnAttribute>>();
                        foreach (Type daoType in _daoTypes.Values)
                        {
                            _columnAttributes.Add(Dao.TableName(daoType), 
                                daoType.GetProperties()
                                    .Where(prop=> prop.HasCustomAttributeOfType<ColumnAttribute>())
                                    .Select(prop=> prop.GetCustomAttributeOfType<ColumnAttribute>())
                                    .ToList());
                        }
                    }
                }
            }
        }

        public override SchemaDefinition Extract()
        {
            if (!_analyzed)
            {
                Analyze();
            }
            return base.Extract();
        }

        public override string GetSchemaName()
        {
            HashSet<string> uniqueSchemaNames = new HashSet<string>();
            foreach (Type daoType in _daoTypes.Values)
            {
                uniqueSchemaNames.Add(Dao.ConnectionName(daoType));
            }

            if (uniqueSchemaNames.Count > 1)
            {
                throw new InvalidOperationException($"Multiple schema names were found in the specified assembly ({Assembly.FullName}) and namespace ({Namespace}): {string.Join(", ", uniqueSchemaNames.ToArray())}");
            }
            else if (uniqueSchemaNames.Count == 0)
            {
                throw new InvalidOperationException($"No dao types were found in the specified namespace ({Namespace}) of the specified assembly ({Assembly.FullName}).");
            }

            return uniqueSchemaNames.FirstOrDefault();
        }

        public override string[] GetTableNames()
        {
            return _daoTypes.Keys.ToArray();
        }

        public override string GetKeyColumnName(string tableName)
        {
            return Dao.GetKeyColumnName(_daoTypes[tableName]);
        }

        public override string[] GetColumnNames(string tableName)
        {
            return _columnAttributes[tableName].Select(c => c.Name).ToArray();
        }

        public override DataTypes GetColumnDataType(string tableName, string columnName)
        {
            ColumnAttribute columnAttribute = GetColumnAttribute(tableName, columnName);

            return DataTypeTranslator.TranslateDataType(columnAttribute.DbDataType);
        }
        
        public override string GetColumnDbDataType(string tableName, string columnName)
        {
            return GetColumnAttribute(tableName, columnName).DbDataType;
        }

        public override string GetColumnMaxLength(string tableName, string columnName)
        {
            return GetColumnAttribute(tableName, columnName).MaxLength;
        }

        public override bool GetColumnNullable(string tableName, string columnName)
        {
            return GetColumnAttribute(tableName, columnName).AllowNull;
        }

        public override ForeignKeyColumn[] GetForeignKeyColumns()
        {
            List<ForeignKeyColumn> foreignKeyColumns = new List<ForeignKeyColumn>();
            foreach (string tableName in _columnAttributes.Keys)
            {
                List<ColumnAttribute> columnAttributes = _columnAttributes[tableName];
                foreignKeyColumns.AddRange(columnAttributes.OfType<ForeignKeyAttribute>().Select(ColumnFromAttribute));
            }

            return foreignKeyColumns.ToArray();
        }
        
        protected override void SetConnectionName(string connectionString)
        {
            // no op
        }

        private ForeignKeyColumn ColumnFromAttribute(ForeignKeyAttribute attribute)
        {
            return new ForeignKeyColumn()
            {
                TableName = attribute.Table,
                Name = attribute.Name,
                AllowNull = attribute.AllowNull,
                ReferenceName = attribute.ForeignKeyName,
                ReferencedKey = attribute.ReferencedKey,
                ReferencedTable = attribute.ReferencedTable
            };
        }
        
        private ColumnAttribute GetColumnAttribute(string tableName, string columnName)
        {
            ColumnAttribute columnAttribute = _columnAttributes[tableName].FirstOrDefault(c => c.Name.Equals(columnName));
            if (columnAttribute == null)
            {
                throw new InvalidOperationException($"Column not found {tableName}.{columnName}");
            }

            return columnAttribute;
        }
    }
}