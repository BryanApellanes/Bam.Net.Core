using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public class JSchemaProperty
    {
        private JSchema _propertyTypeJSchema;

        public JSchemaProperty(JSchema propertyType)
        {
            _propertyTypeJSchema = propertyType;
        }
        
        public string PropertyName { get; set; }
    }
}