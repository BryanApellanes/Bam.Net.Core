using System;
using System.Linq;

namespace Bam.Net.Data.GraphQL
{
    public class TypeModel
    {
        public Type Type { get; set; }
        
        public string SchemaName { get; set; }
        
        public string TypeName
        {
            get { return Type.Name; }
        }

        public string FullyQualifiedTypeName
        {
            get { return $"{Type.Namespace}.{Type.Name}"; }
        }
        
        public string UsingStatements
        {
            get { return $"using {Type.Namespace};\r\n"; }
        }

        public string ToNameSpace
        {
            get;
            set;
        }
        
        public string PluralTypeName
        {
            get { return TypeName.Pluralize(); }
        }
        
        public PropertyModel[] Properties
        {
            get { return GraphQLTypeGenerator.GetPropertyModels(Type).ToArray(); }
        }

        public QueryArgumentModel[] QueryArguments
        {
            get { return GraphQLTypeGenerator.GetQueryArguments(Type).ToArray(); }
        }
    }
}