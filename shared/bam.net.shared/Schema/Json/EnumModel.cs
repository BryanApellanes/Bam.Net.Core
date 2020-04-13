using System.Linq;

namespace Bam.Net.Schema.Json
{
    public class EnumModel
    {
        public EnumModel()
        {
        }

        public EnumModel(JSchemaClass jSchemaClass, string nameSpace)
        {
            Namespace = nameSpace;
            Name = jSchemaClass.ClassName;
            Values = jSchemaClass.GetEnumNames().ToArray();
        }

        public string Namespace { get; set; }
        public string Name { get; set; }
        public string[] Values { get; set; }
    }
}