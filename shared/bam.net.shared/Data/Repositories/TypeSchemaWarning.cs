using System;
using Bam.Net.Data.Schema;

namespace Bam.Net.Data.Repositories
{
    public class TypeSchemaWarning
    {
        public TypeSchemaWarnings Warning { get; set; }
        public Type ParentType { get; set; }
        public Type ForeignKeyType { get; set; }

        public override string ToString()
        {
            string fkString = ForeignKeyType != null ? $", ForeignKeyType={ForeignKeyType?.Name}" : "";
            return $"{Warning.ToString()}: ParentType={ParentType?.Name ?? "null"}" + fkString;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is TypeSchemaWarning typeSchemaWarning)
            {
                return typeSchemaWarning.Warning == Warning && typeSchemaWarning.ParentType == ParentType &&
                       typeSchemaWarning.ForeignKeyType == ForeignKeyType;
            }

            return false;
        }

        public static TypeSchemaWarning FromEventArgs(TypeSchemaWarningEventArgs args)
        {
            return new TypeSchemaWarning
            {
                Warning = args.Warning,
                ParentType =  args.ParentType,
                ForeignKeyType = args.ForeignKeyType
            };
        }
        
        public TypeSchemaWarningEventArgs ToEventArgs()
        {
            return TypeSchemaWarningEventArgs.FromTypeSchemaWarning(this);
        }
    }
}