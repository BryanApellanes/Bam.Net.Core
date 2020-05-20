using System;

namespace Bam.Net.Schema.Json
{
    public class JSchemaEnumGeneratorEventArgs : EventArgs
    {
        public JSchemaEnumGeneratorEventArgs(JSchemaEnumGenerator generator)
        {
            this.Generator = generator;
        }
        public Exception Exception { get; set; }
        public JSchemaEnumGenerator Generator { get; set; }
        public JSchemaSchemaDefinition JSchemaSchemaDefinition { get; set; }
        public EnumModel Model { get; set; }
        public string CodeFile { get; set; }
    }
}