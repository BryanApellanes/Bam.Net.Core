using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Schema.Json
{
    public class JSchemaDescriptor
    {
        private JSchema _jSchema;
        public JSchemaDescriptor(JSchema jSchema)
        {
            _jSchema = jSchema;
        }

        public IDictionary<string, JSchemaDescriptor> Properties
        {
            get
            {
                if (_jSchema?.Properties != null)
                {
                    
                }
                throw new NotImplementedException();
            } 
            
        }
        
        public IDictionary<string, JToken> ExtensionData { get; set; }

        private IDictionary<string, JSchemaDescriptor> GetProperties(JSchema jSchema)
        {
            //if(_jSchema!.Properties != null)
            throw new NotImplementedException();
        }
    }
}