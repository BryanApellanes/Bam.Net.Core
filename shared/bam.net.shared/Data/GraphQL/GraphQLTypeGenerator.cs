using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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

        public GraphQLTypeGenerator(GraphQLGenerationConfig config) : this()
        {
            Config = config;
            SourceDirectoryPath = config.WriteSourceTo;
            AssemblyName = config.ToNameSpace;
            Assembly assembly = Assembly.LoadFrom(config.TypeAssembly);
            AddTypes(assembly.GetTypes().Where(type =>
                RuntimeSettings.ClrTypeFilter(type) && type != null && type.Namespace != null &&
                type.Namespace.Equals(config.FromNameSpace)).ToArray());
        }

        public GraphQLGenerationConfig Config { get; set; }
        
        protected HandlebarsTemplateRenderer HandlebarsTemplateRenderer { get; set; }
        
        public HashSet<Type> Types { get; private set; }

        public void AddType(Type type)
        {
            Types.Add(type);
        }

        public void AddTypes(params Type[] types)
        {
            types.Each(AddType);
        }
        
        public override void WriteSource(string writeSourceDir)
        {
            List<TypeModel> typeModels = new List<TypeModel>();
            Parallel.ForEach(Types, type =>
            {
                TypeModel typeModel = new TypeModel {Type = type, SchemaName = Config.SchemaName, ToNameSpace = Config.ToNameSpace};
                typeModels.Add(typeModel);
                string code = HandlebarsTemplateRenderer.Render("GraphQLType", typeModel);
                string writeToFile = Path.Combine(writeSourceDir, $"{type.Name}Graph.cs");
                code.SafeWriteToFile(writeToFile, true);
            });
            SchemaModel schemaModel = new SchemaModel()
            {
                SchemaName = Config.SchemaName, DataTypes = typeModels.ToArray(),
                UsingStatements = GetUsingStatements(typeModels.Select(t => t.Type)),
                ToNameSpace = Config.ToNameSpace
            };
            string contextCode = HandlebarsTemplateRenderer.Render("GraphQLQueryContext", schemaModel);           
            contextCode.SafeWriteToFile(Path.Combine(writeSourceDir, $"{Config.SchemaName}.cs"), true);
        }

        public override Assembly CompileAssembly(out byte[] bytes)
        {
            RoslynCompiler compiler = new RoslynCompiler();
            foreach (Type type in Types)
            {
                compiler.AddAssemblyReference(type.Assembly.Location);
            }
            
            compiler.AddAssemblyReference(typeof(GraphType).Assembly.Location);

            bytes = compiler.Compile(AssemblyName ?? 8.RandomLetters(), new DirectoryInfo(SourceDirectoryPath));
            return Assembly.Load(bytes);
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
            return type.GetProperties().Where(p => !p.GetGetMethod().IsStatic && (!p.IsEnumerable() || p.PropertyType == typeof(string)));
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

        internal static string GetUsingStatements(IEnumerable<Type> types)
        {
            StringBuilder result = new StringBuilder();
            HashSet<string> hashSet = new HashSet<string>();
            types.Each(t => hashSet.Add(t.Namespace));
            foreach (string ns in hashSet)
            {
                result.AppendLine($"using {ns};");
            }

            return result.ToString();
        }
    }
}