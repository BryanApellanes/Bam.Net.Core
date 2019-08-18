using System;
using System.IO;
using Bam.Net.Data;
using Bam.Net.Data.Schema;

namespace Bam.Net.Application.Models
{
    public class ModelSchemaManager : SchemaManager
    {
        public ModelSchemaManager()
        {
        }
        
        public bool IncludeUuid { get; set; }
        public bool IncludeCuid { get; set; }

        public void AddUuidColumns()
        {
            PreColumnAugmentations.Add(new AddColumnAugmentation { ColumnName = "Uuid", DataType = DataTypes.String, AllowNull = true });
        }

        public void AddCuidColumns()
        {
            PreColumnAugmentations.Add(new AddColumnAugmentation { ColumnName = "Cuid", DataType = DataTypes.String, AllowNull = true });
        }
        
        public ModelSchema ModelSchema{ get; set; }

        public SchemaDefinition ConvertModelSchema()
        {
            return ConvertModelSchema(ModelSchema);
        }

        public SchemaDefinition ConvertModelSchema(ModelSchema modelSchema)
        {
            SchemaDefinition schemaDefinition = new SchemaDefinition {Name = modelSchema.Name};
            foreach (ModelDefinition definition in modelSchema.Definitions)
            {
                AddTable(definition.Name);
                foreach (ModelProperty property in definition.Properties)
                {
                    AddColumn(definition.Name, property.Name, property.Type);
                }
            }

            foreach (ModelDefinition currentModel in modelSchema.Definitions)
            {
                foreach (string referencingModel in currentModel.HasMany)
                {
                    string referencingColumnName = $"{currentModel.Name}Id";
                    if (!GetTable(currentModel.Name).HasColumn(referencingColumnName))
                    {
                        AddColumn(referencingModel, referencingColumnName, DataTypes.ULong);
                    }
                    SetForeignKey(currentModel.Name, referencingModel, referencingColumnName, "Id");
                }

                foreach (string referencedModel in currentModel.BelongsTo)
                {
                    string referencingColumnName = $"{referencedModel}Id";
                    if(!GetTable(referencedModel).HasColumn(referencingColumnName))
                    {
                        AddColumn(currentModel.Name, referencingColumnName, DataTypes.ULong);
                    }
                    SetForeignKey(referencedModel, currentModel.Name, referencingColumnName, "Id");
                }
            }
            
            return schemaDefinition;
        }

        public ModelSchema LoadModelSchema(string filePath)
        {
            ModelSchema = new FileInfo(filePath).FullName.FromJsonFile<ModelSchema>();
            return ModelSchema;
        }

        public void SaveModelSchema(string path)
        {
            SaveModelSchema(ModelSchema, path);
        }
        
        public static void SaveModelSchema(ModelSchema modelSchema, string path)
        {
            modelSchema.ToJsonFile(path);
        }
    }
}