using System.Reflection;
using Bam.Net.Data.Dynamic;
using Bam.Net.Data.Schema;
using Google.Protobuf.WellKnownTypes;

namespace Bam.Net.Schema.Json
{
    public class JSchemaDaoAssemblyGenerator: DaoAssemblyGenerator
    {
        private DaoAssemblyGenerator _daoGenerator;
        public JSchemaDaoAssemblyGenerator(JSchemaSchemaDefinitionGenerator schemaDefinitionGenerator = null, JSchemaEnumGenerator enumGenerator = null)
        {
            JSchemaSchemaDefinitionGenerator = schemaDefinitionGenerator ?? new JSchemaSchemaDefinitionGenerator(new SchemaManager(), new JSchemaClassManager());
            EnumGenerator = enumGenerator ?? new JSchemaEnumGenerator();
            _daoGenerator = new DaoAssemblyGenerator();
            Namespace = $"{nameof(JSchemaDaoAssemblyGenerator)}.Generated";
        }
        
        public JSchemaSchemaDefinitionGenerator JSchemaSchemaDefinitionGenerator { get; set; }
        public JSchemaEnumGenerator EnumGenerator { get; set; }
        public string Workspace { get; set; }
        public string JsonSchemaRootPath { get; set; }
        public string Namespace { get; set; }
        public override void GenerateSource(string workspace)
        {
            // Set the schema before calling base.GenerateSource so the Schema is set before generation
            JSchemaSchemaDefinition schemaDefinition = JSchemaSchemaDefinitionGenerator.GenerateSchemaDefinition(JsonSchemaRootPath);
            Schema = schemaDefinition;
            
            base.GenerateSource(workspace);

            EnumGenerator.Workspace = workspace;
            EnumGenerator.GenerateEnums(schemaDefinition, Namespace);
        }
    }
}