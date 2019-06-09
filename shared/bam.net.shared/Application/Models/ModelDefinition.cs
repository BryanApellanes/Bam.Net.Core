namespace Bam.Net.Application.Models
{
    public class ModelDefinition
    {
        public string Name { get; set; }
        public ModelProperty[] Properties { get; set; }
        
        /// <summary>
        /// An array of ModelDefinition names that reference this ModelDefinition.
        /// </summary>
        public string[] HasMany { get; set; }
        
        /// <summary>
        /// An array of ModelDefinition names that this ModelDefinition references.
        /// </summary>
        public string[] BelongsTo { get; set; }
    }
}