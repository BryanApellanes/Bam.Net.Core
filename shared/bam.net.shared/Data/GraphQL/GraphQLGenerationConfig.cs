namespace Bam.Net.Data.GraphQL
{
    public class GraphQLGenerationConfig
    {
        string _schemaName;

        public string SchemaName
        {
            get
            {
                return _schemaName ?? $"{TypeAssembly}{FromNameSpace}{ToNameSpace}".Sha1();
            }
            set
            {
                _schemaName = value;
            }
        }

        /// <summary>
        /// The path to the assembly containing types to generate graphql types for.
        /// </summary>
        public string TypeAssembly { get; set; }
        
        /// <summary>
        /// The namespace containing types to generate graphql types for.
        /// </summary>
        public string FromNameSpace { get; set; }
        
        /// <summary>
        /// The namespace to write generated types to.
        /// </summary>
        public string ToNameSpace { get; set; }
        
        /// <summary>
        /// The path of the directory to write source files to.
        /// </summary>
        public string WriteSourceTo { get; set; }
    }
}