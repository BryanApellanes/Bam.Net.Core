namespace Bam.Net.Data.GraphQL
{
    public class SchemaModel
    {
        public string SchemaName { get; set; }
        public string UsingStatements { get; set; }
        public string ToNameSpace { get; set; }
        public TypeModel[] DataTypes { get; set; }
    }
}