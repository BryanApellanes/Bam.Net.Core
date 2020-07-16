using System;

namespace Bam.Net.Data.Repositories
{
    public class TypeSchemaWarningEventArgs: EventArgs
    {
        public TypeSchemaWarnings Warning { get; set; }
        public Type ParentType { get; set; }
        public Type ForeignKeyType { get; set; }
        public string ParentTypeName => ParentType.Name;
        public string ForeignKeyTypeName => ForeignKeyType.Name;

        public static TypeSchemaWarningEventArgs FromTypeSchemaWarning(TypeSchemaWarning warning)
        {
            return new TypeSchemaWarningEventArgs()
            {
                ParentType = warning.ParentType,
                ForeignKeyType = warning.ForeignKeyType,
                Warning = warning.Warning
            };
        }
    }
}