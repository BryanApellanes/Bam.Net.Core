/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Runtime.CompilerServices;

namespace Bam.Net.Data.Repositories
{
	/// <summary>
	/// A Data Transfer Object.  Represents the properties
	/// of Dao types without the associated methods.  
	/// </summary>
	public partial class Dto
    {
        public const string DefaultNamespace = "Bam.Net.Data.Dto";
        public string ToJson()
        {
            return Extensions.ToJson(this);
        }
        
        /// <summary>
        /// Get the associated Dto types for the 
        /// Dao types in the specified daoAssembly.
        /// </summary>
        /// <param name="daoAssembly"></param>
        /// <returns></returns>
        public static Type[] GetTypesFromDaos(Assembly daoAssembly)
        {
            GeneratedAssemblyInfo assemblyInfo = GetGeneratedDtoAssemblyInfo(daoAssembly);

            return assemblyInfo.GetAssembly().GetTypes();
        }

        /// <summary>
        /// Get a generated Dto type for the specified Dao instance.
        /// The Dto type will only have properties that match the columns
        /// of the Dao.
        /// </summary>
        /// <param name="daoInstance"></param>
        /// <returns></returns>
        public static Type TypeFor(Dao daoInstance)
        {
            return TypeFor(daoInstance.GetType());
        }

        /// <summary>
        /// Get the associated Dto type for the specified
        /// daoType
        /// </summary>
        /// <param name="daoType"></param>
        /// <returns></returns>
        public static Type TypeFor(Type daoType)
        {
            return GetTypesFromDaos(daoType.Assembly).FirstOrDefault(t => t.Name.Equals(daoType.Name));
        }

        /// <summary>
        /// Copy the specified Dao instance as an equivalent Dto instance
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static object Copy(Dao instance)
        {
            return instance.CopyAs(TypeFor(instance));
        }

        static object _genLock = new object();
        /// <summary>
        /// Generates an assembly containing Dto's that represent all the 
        /// Dao's found in the specified daoAssembly.  A Dto or (DTO) is
        /// a Data Transfer Object and represents only the properties of 
        /// a Dao.  A Dao or (DAO) is a Data Access Object that represents
        /// both properties and methods to create, retrieve, update and delete. 
        /// </summary>
        /// <param name="daoAssembly"></param>
        /// <returns></returns>
        public static GeneratedAssemblyInfo GetGeneratedDtoAssemblyInfo(Assembly daoAssembly, string fileName = null)
        {
            lock (_genLock)
            {
                DaoToDtoGenerator generator;
                string defaultFileName = GetDefaultFileName(daoAssembly, out generator);
                fileName = fileName ?? defaultFileName;
                GeneratedAssemblyInfo assemblyInfo = GeneratedAssemblyInfo.GetGeneratedAssembly(fileName, generator);
                return assemblyInfo;
            }
        }

        public static string GetDefaultFileName(Assembly daoAssembly)
        {
            DaoToDtoGenerator ignore;
            return GetDefaultFileName(daoAssembly, out ignore);
        }

        public static string GetDefaultFileName(Assembly daoAssembly, out DaoToDtoGenerator generator)
        {
            generator = new DaoToDtoGenerator(daoAssembly);
            string fileName = generator.GetDefaultFileName();
            return fileName;
        }

        public static dynamic InstanceFor(string typeName, Dictionary<object, object> dictionary)
        {
            return InstanceFor(DefaultNamespace, typeName, dictionary);
        }
        
        public static dynamic InstanceFor(string nameSpace, string typeName, Dictionary<object, object> dictionary)
        {
            Type type = TypeFor(nameSpace, typeName, dictionary);
            return dictionary.ToInstance(type);
        }

        public static Type TypeFor(string typeName, Dictionary<object, object> dictionary)
        {
            return TypeFor(DefaultNamespace, typeName, dictionary);
        }
        
        public static Type TypeFor(string nameSpace, string typeName, Dictionary<object, object> dictionary)
        {
            return AssemblyFor(nameSpace, typeName, dictionary).GetTypes().FirstOrDefault(t => t.Name.Equals(DtoModel.CleanTypeName(typeName)));
        }

        public static Assembly AssemblyFor(string typeName, Dictionary<object, object> dictionary)
        {
            return AssemblyFor(DefaultNamespace, typeName, dictionary);
        }

        public static Assembly AssemblyFor(string nameSpace, string typeName, Dictionary<object, object> dictionary)
        {
            return AssemblyFor(nameSpace, nameSpace, typeName, dictionary);
        }
        
        static Dictionary<string, Assembly> _dtoAssemblies = new Dictionary<string, Assembly>();
        static object _dtoAssemblyLock = new object();
        public static Assembly AssemblyFor(string assemblyName, string nameSpace, string typeName, Dictionary<object, object> dictionary)
        {
            nameSpace = nameSpace ?? DefaultNamespace;
            DtoModel dtoModel = new DtoModel(nameSpace, typeName, dictionary);
            string dtoSrc = dtoModel.Render();
            string key = dtoSrc.Sha256();
            lock (_dtoAssemblyLock)
            {
                if (!_dtoAssemblies.ContainsKey(key))
                {
                    RoslynCompiler compiler = new RoslynCompiler();
                    _dtoAssemblies.Add(key, compiler.CompileAssembly(assemblyName, dtoSrc));
                }
            }

            return _dtoAssemblies[key];
        }
        
        public static void WriteRenderedDto(string nameSpace, string writeSourceTo, Type daoType, Func<PropertyInfo, bool> propertyFilter)
        {
            string typeName = Dao.TableName(daoType);
            typeName = string.IsNullOrEmpty(typeName) ? daoType.Name : typeName;
            DtoModel dtoModel = new DtoModel(nameSpace, typeName, daoType.GetProperties().Where(propertyFilter).Select(pi=> new DtoPropertyModel(pi)).ToArray());
            WriteRenderedDto(writeSourceTo, dtoModel);
        }
        
        public static void WriteRenderedDto(string nameSpace, string writeSourceTo, Type dynamicDtoType)
        {
            DtoModel dtoModel = new DtoModel(dynamicDtoType, nameSpace);
            WriteRenderedDto(writeSourceTo, dtoModel);
        }

        private static void WriteRenderedDto(string writeSourceTo, DtoModel dtoModel)
        {
            string csFile = "{0}.cs"._Format(dtoModel.TypeName);
            FileInfo csFileInfo = new FileInfo(Path.Combine(writeSourceTo, csFile));
            if (!csFileInfo.Directory.Exists)
            {
                csFileInfo.Directory.Create();
            }
            using (StreamWriter sw = new StreamWriter(csFileInfo.FullName))
            {
                sw.Write(dtoModel.Render());
            }
        }
    }
}
