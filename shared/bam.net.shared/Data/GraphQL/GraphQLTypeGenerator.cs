using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bam.Net.Data.Repositories;
using Bam.Net.Data.Schema;
using GraphQL.Types;
using GraphQLParser;

namespace Bam.Net.Data.GraphQL
{
    public class GraphQLTypeGenerator : AssemblyGenerator
    {
        public GraphQLTypeGenerator()
        {
            Types = new HashSet<Type>();
            HandlebarsTemplateRenderer = new HandlebarsTemplateRenderer();
        }

        protected HandlebarsTemplateRenderer HandlebarsTemplateRenderer { get; set; }
        
        /// <summary>
        /// The name of the assembly to generate, if this values is null a random name is generated.
        /// </summary>
        public string AssemblyName { get; set; }
        
        public HashSet<Type> Types { get; private set; }

        public void AddType(Type type)
        {
            Types.Add(type);
        }

        public void AddTypes(params Type[] types)
        {
            types.Each(type => AddType(type));
        }
        
        public override void WriteSource(string writeSourceDir)
        {
            Parallel.ForEach(Types, type =>
            {
                string code = HandlebarsTemplateRenderer.Render("GraphQLType", new TypeModel {Type = type});
                string writeToFile = Path.Combine(writeSourceDir, $"{type.Name}Graph.cs");
                code.SafeWriteToFile(writeToFile, true);
            });
        }

        public override Assembly Compile()
        {
            RoslynCompiler compiler = new RoslynCompiler();
            return compiler.Compile(AssemblyName ?? 8.RandomLetters(), new DirectoryInfo(SourceDirectoryPath));
        }

        internal static IEnumerable<PropertyModel> GetPropertyModels(Type type)
        {
            foreach (PropertyModel propertyModel in GetProperties(type).Select(p => new PropertyModel(p)))
            {
                yield return propertyModel;
            }

            foreach (PropertyModel propertyModel in GetEnumerableProperties(type).Select(p => new PropertyModel(p)))
            {
                yield return propertyModel;
            }
        }
        
        internal static IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return type.GetProperties().Where(p => !p.IsEnumerable() || p.PropertyType == typeof(string));
        }

        internal static IEnumerable<PropertyInfo> GetEnumerableProperties(Type type)
        {
            return type.GetProperties().Where(p => p.IsEnumerable() && p.PropertyType != typeof(string));
        }
        
        internal static string GetGraphTypeName(PropertyInfo property)
        {
            if (property.PropertyType == typeof(int))
            {
                return typeof(IntGraphType).Name;
            }

            if (property.PropertyType == typeof(float))
            {
                return typeof(FloatGraphType).Name;
            }

            if (property.PropertyType == typeof(bool))
            {
                return typeof(BooleanGraphType).Name;
            }

            if (property.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase))
            {
                return typeof(IdGraphType).Name;
            }

            if (property.PropertyType.IsEnumerable() && property.PropertyType != typeof(string))
            {
                return "ListGraphType<StringGraphType>";
            }
            
            return typeof(StringGraphType).Name;
        }

        internal static IEnumerable<QueryArgumentModel> GetQueryArguments(Type type)
        {
            PropertyInfo[] properties = GetProperties(type).ToArray();
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo prop = properties[i];
                QueryArgumentModel model = new QueryArgumentModel{GraphType = GetGraphTypeName(prop), Name = prop.Name };
                if (i != properties.Length - 1)
                {
                    model.Separator = ",\r\n\t\t\t\t\t";
                }

                yield return model;
            }
        }
    }
}