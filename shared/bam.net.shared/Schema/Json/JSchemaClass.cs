using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public class JSchemaClass
    {
        private JSchema _jSchema;
        public JSchemaClass(JSchema jSchema)
        {
            _jSchema = jSchema;
        }
    }
}