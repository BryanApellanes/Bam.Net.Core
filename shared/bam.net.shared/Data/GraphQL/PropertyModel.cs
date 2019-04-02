using System.Reflection;

namespace Bam.Net.Data.GraphQL
{
    public class PropertyModel
    {
        public PropertyModel(PropertyInfo property)
        {
            PropertyInfo = property;
        }
        
        public PropertyInfo PropertyInfo { get; set; }

        public string PropertyName
        {
            get { return PropertyInfo.Name; }
        }

        public string PropertyGraphType
        {
            get { return GraphQLTypeGenerator.GetGraphTypeName(PropertyInfo); }
        }
    }
}