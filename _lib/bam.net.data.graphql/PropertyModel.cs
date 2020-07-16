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

        public string PropertyName => PropertyInfo.Name;

        public string PropertyGraphType => GraphQLTypeGenerator.GetGraphTypeName(PropertyInfo);
    }
}