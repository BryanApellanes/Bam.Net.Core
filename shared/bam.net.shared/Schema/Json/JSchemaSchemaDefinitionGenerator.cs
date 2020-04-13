using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bam.Net.Data;
using Bam.Net.Data.Schema;
using Bam.Net.Logging;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    /// <summary>
    /// A class used to generate a SchemaDefinition from one or more JSchemas
    /// </summary>
    public class JSchemaSchemaDefinitionGenerator: Loggable
    {
        public JSchemaSchemaDefinitionGenerator(SchemaManager schemaManager, JSchemaClassManager jSchemaClassManager)
        {
            Args.ThrowIfNull(jSchemaClassManager, "jSchemaClassManager");
            SchemaManager = schemaManager;
            JSchemaClassManager = jSchemaClassManager;
            JsonSchemaRootPath = "./JsonSchema/";
            Logger = Log.Default;
            SchemaManager.ManageSchema(Path.Combine(BamProfile.Data, "SchemaDefinition_FromJSchemas.json"));
        }
        
        public string JsonSchemaRootPath { get; set; }
        
        [Verbosity(VerbosityLevel.Information)]
        public event EventHandler GenerateSchemaDefinitionStarted;
        
        [Verbosity(VerbosityLevel.Information)]
        public event EventHandler GenerateSchemaDefinitionComplete;
        
        [Verbosity(VerbosityLevel.Error)]
        public event EventHandler GenerateSchemaDefinitionException;
        
        [Verbosity(VerbosityLevel.Information)]
        public event EventHandler AddingTable;
        
        [Verbosity(VerbosityLevel.Information)]
        public event EventHandler AddedTable;

        [Verbosity(VerbosityLevel.Information)]
        public event EventHandler AddingColumn;

        [Verbosity(VerbosityLevel.Information)]
        public event EventHandler AddedColumn;

        [Verbosity(VerbosityLevel.Information)]
        public event EventHandler AddingForeignKey;

        [Verbosity(VerbosityLevel.Information)]
        public event EventHandler AddedForeignKey;
        public ILogger Logger { get; private set; }
        public SchemaManager SchemaManager { get; private set; }
        public JSchemaClassManager JSchemaClassManager { get; private set; }

        public JSchemaSchemaDefinition GenerateSchemaDefinition()
        {
            return GenerateSchemaDefinition(JsonSchemaRootPath);
        }
        
        public JSchemaSchemaDefinition GenerateSchemaDefinition(string jsonSchemaRoot)
        {
            HashSet<JSchemaClass> jSchemaClasses = new HashSet<JSchemaClass>();
            try
            {
                SchemaManager.AutoSave = false;
                FireEvent(GenerateSchemaDefinitionStarted, this, GetEventArgs());
                jSchemaClasses = JSchemaClassManager.GetAllJSchemaClasses(jsonSchemaRoot);

                // for all classes
                foreach (JSchemaClass jSchemaClass in jSchemaClasses.Where(js=> !js.IsEnum))
                {
                    string tableName = jSchemaClass.ClassName;
                    //  add a table for the class
                    FireEvent(AddingTable, GetEventArgs(tableName));
                    SchemaManager.AddTable(tableName, tableName);
                    FireEvent(AddedTable, GetEventArgs(tableName));
                    //  add a column for all the value properties
                    foreach (JSchemaProperty property in jSchemaClass.ValueProperties)
                    {
                        string columnName = property.PropertyName;
                        FireEvent(AddingColumn, GetEventArgs(tableName, columnName));
                        SchemaManager.AddColumn(tableName, columnName, GetDataType(property));
                        FireEvent(AddedColumn, GetEventArgs(tableName, columnName));
                    }
                }

                //  
                // then
                foreach (JSchemaClass jSchemaClass in jSchemaClasses.Where(js=> !js.IsEnum))
                {
                    string tableName = jSchemaClass.ClassName;
                    //   add a foreign key for all the classes that have object properties
                    foreach (JSchemaProperty objectProperty in jSchemaClass.ObjectProperties)
                    {
                        // add a convention based referencing column
                        JSchemaClass classOfProperty = objectProperty.ClassOfProperty;
                        string targetTable = classOfProperty.ClassName;
                        string referencingColumnName = $"{classOfProperty.ClassName}Id";
                        FireEvent(AddingForeignKey, GetEventArgs(tableName, referencingColumnName));
                        SchemaManager.AddColumn(tableName, referencingColumnName, DataTypes.ULong);
                        SchemaManager.SetForeignKey(targetTable, tableName, referencingColumnName);
                        FireEvent(AddedForeignKey, GetEventArgs(tableName, referencingColumnName));
                    }
                    
                    //   add a foreign key for all the classes of array properties
                    foreach (JSchemaProperty arrayProperty in jSchemaClass.ArrayProperties)
                    {
                        JSchemaClass classOfArrayItems = arrayProperty.ClassOfArrayItems;
                        string tableToAddFkTo = classOfArrayItems.ClassName;
                        string targetTable = tableName;
                        string referencingColumnName = $"{jSchemaClass.ClassName}Id";
                        FireEvent(AddingForeignKey, GetEventArgs(tableToAddFkTo, referencingColumnName));
                        SchemaManager.AddColumn(tableToAddFkTo, referencingColumnName, DataTypes.ULong);
                        SchemaManager.SetForeignKey(targetTable, tableToAddFkTo, referencingColumnName);
                    }
                }

                SchemaManager.Save();
                FireEvent(GenerateSchemaDefinitionComplete, GetEventArgs(SchemaManager.GetCurrentSchema()));
            }
            catch (Exception ex)
            {
                Logger.Error("Error generating SchemaDefinition: {0}", ex.Message);
                FireEvent(GenerateSchemaDefinitionException, this, GetEventArgs(ex));
            }

            return new JSchemaSchemaDefinition(SchemaManager.CurrentSchema, jSchemaClasses);
        }

        private EventArgs GetEventArgs(Exception ex)
        {
            return GetEventArgs(null, null, ex);
        }

        private EventArgs GetEventArgs(SchemaDefinition schemaDefinition)
        {
            JSchemaSchemaDefinitionGeneratorEventArgs args = (JSchemaSchemaDefinitionGeneratorEventArgs) GetEventArgs();
            args.SchemaDefinition = schemaDefinition;
            return args;
        }
        
        private EventArgs GetEventArgs(string tableName = null, string columnName = null, Exception ex = null)
        {
            JSchemaSchemaDefinitionGeneratorEventArgs args = new JSchemaSchemaDefinitionGeneratorEventArgs(this)
            {
                TableName = tableName, ColumnName = columnName, Exception = ex
            };

            return args;
        }
        
        private DataTypes GetDataType(JSchemaProperty property)
        {
            Args.ThrowIfNull(property, "property");
            switch (property.Type)
            {
                case JSchemaType.None:
                case JSchemaType.String:
                    return DataTypes.String;
                case JSchemaType.Number:
                    return DataTypes.ULong; 
                case JSchemaType.Integer:
                    return DataTypes.Int; 
                case JSchemaType.Boolean:
                    return DataTypes.Boolean; 
                case JSchemaType.Object:
                case JSchemaType.Array: 
                case JSchemaType.Null: 
                default:
                    Args.Throw<ArgumentException>("JSchemaType of property ({0}).({1}) is not supported: {2}",
                        property.DeclaringClassName, property.PropertyName, property.Type);
                    break;
            }

            return DataTypes.String;
        }
    }
}