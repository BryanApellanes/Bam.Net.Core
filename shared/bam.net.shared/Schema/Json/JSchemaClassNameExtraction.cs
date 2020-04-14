using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public class JSchemaClassNameExtraction
    {
        public static implicit operator string(JSchemaClassNameExtraction extraction)
        {
            return extraction.ClassName;
        }

        public JSchemaClassNameExtraction()
        {
        }
        
        public JSchemaClassNameExtraction(string className)
        {
            ClassName = className;
        }
        
        public JSchema JSchema { get; set; }
        public JSchemaClass JSchemaClass { get; set; }
        public JSchemaClassManager JSchemaClassManager { get; set; }
        
        /// <summary>
        /// The list of properties that the JSchemaClassManager checked.
        /// </summary>
        public string[] ClassNameProperties { get; set; }
        public string ClassName { get; set; }
    }
}