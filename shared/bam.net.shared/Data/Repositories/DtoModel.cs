/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Reflection;
using Bam.Net.Data.Schema;
using Bam.Net.Presentation.Handlebars;
using Bam.Net.ServiceProxy;

namespace Bam.Net.Data.Repositories
{
    public partial class DtoModel
	{
		public DtoModel(Type dynamicDtoType, string nameSpace)
		{
			TypeName = dynamicDtoType.Name;
			List<string> properties = new System.Collections.Generic.List<string>();
			foreach(PropertyInfo p in dynamicDtoType.GetProperties())
			{
				Type type = (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) ? Nullable.GetUnderlyingType(p.PropertyType) : p.PropertyType;
				properties.Add("\t\tpublic {0} {1} {{get; set;}}\r\n"._Format(type.Name, p.Name));
			}
			Properties = properties.ToArray();
			DtoType = dynamicDtoType;
			Namespace = nameSpace;
			LoadEmbeddedHandlebars();
			CleanTypeName();
		}

        public DtoModel(string nameSpace, string typeName, params DtoPropertyModel[] propertyModels)
        {
            List<string> properties = new List<string>();
            foreach(DtoPropertyModel p in propertyModels)
            {
                properties.Add("\t\tpublic {0} {1} {{get; set;}}\r\n"._Format(p.PropertyType, p.PropertyName));
            }
            TypeName = typeName;
            Properties = properties.ToArray();
            Namespace = nameSpace;
            LoadEmbeddedHandlebars();
            CleanTypeName();
        }

        public DtoModel(string nameSpace, string typeName, Dictionary<object, object> propertyValues)
        {
	        TypeName = typeName;
	        List<string> propertyNames = new List<string>();
	        foreach (object key in propertyValues.Keys)
	        {
		        string propertyName = key.ToString();
		        string propertyType = propertyValues[key].GetType().Name;
		        propertyNames.Add($"\t\tpublic {propertyType} {propertyName} {{get; set;}}\r\n");
	        }

	        Properties = propertyNames.ToArray();
	        Namespace = nameSpace;
	        LoadEmbeddedHandlebars();
	        CleanTypeName();
        }
        
		public string TypeName { get; set; }
		public string Namespace { get; set; }
		public string[] Properties { get; set; }

		public Type DtoType { get; set; }

		private void LoadEmbeddedHandlebars()
		{
			Net.Handlebars.HandlebarsEmbeddedResources = new HandlebarsEmbeddedResources(this.GetType().Assembly);
		}

		private void CleanTypeName()
		{
			TypeName = CleanTypeName(TypeName);
		}

		internal static string CleanTypeName(string typeName)
		{
			return typeName.Replace(".", "_");
		}
	}
}
