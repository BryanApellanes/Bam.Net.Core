using Bam.Net.Data.Dynamic.Data;
using Bam.Net.Data.Dynamic.Data.Dao.Repository;
using Bam.Net.Data.Repositories;
using Bam.Net.Logging;
using Bam.Net.Logging.Counters;
using CsvHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using YamlDotNet.Serialization;
using Timer = Bam.Net.Logging.Counters.Timer;

namespace Bam.Net.Data.Dynamic
{
    /// <summary>
    /// Creates type definitions for json and yaml strings.
    /// </summary>
    public partial class DynamicTypeManager: Loggable
    {
        public DynamicTypeManager() : this(new DynamicTypeDataRepository(), DataProvider.Current)
        { }

        public DynamicTypeManager(DynamicTypeDataRepository descriptorRepository, IDataDirectoryProvider directorySettings, ICompiler compiler = null)
        {
            DataDirectorySettings = directorySettings;
            Compiler = compiler ?? new RoslynCompiler();
            SetReferenceAssemblies();

            descriptorRepository.EnsureDaoAssemblyAndSchema();
            DynamicTypeNameResolver = new DynamicTypeNameResolver();
            DynamicTypeDataRepository = descriptorRepository;
        }

        public ICompiler Compiler { get; set; }
        public IDataDirectoryProvider DataDirectorySettings { get; set; }
        public DynamicTypeNameResolver DynamicTypeNameResolver { get; set; }
        public DynamicTypeDataRepository DynamicTypeDataRepository { get; set; }
        
        /// <summary>
        /// Write source code to the specified appData folder and return
        /// the source as well.
        /// </summary>
        /// <param name="appData">The application data.</param>
        /// <param name="nameSpace">The name space.</param>
        /// <returns></returns>
        public string GenerateSource(DirectoryInfo appData, string nameSpace = null)
        {
            nameSpace = nameSpace ?? DynamicNamespaceDescriptor.DefaultNamespace;

            string source = GenerateSource(nameSpace);
            WriteSource(appData, source);

            return source;
        }

        /// <summary>
        /// Generates classes to represent data found in AppData/json and AppData/yaml placing the assembly 
        /// fil into the _gen/bin directory of the speicifed appData folder.
        /// </summary>
        /// <param name="appData">The application data.</param>
        /// <param name="nameSpace">The name space.</param>
        /// <returns></returns>
        public Assembly GenerateAssembly(DirectoryInfo appData, string nameSpace = null)
        {
            nameSpace = nameSpace ?? DynamicNamespaceDescriptor.DefaultNamespace;

            byte[] assembly = GenerateAssembly(out string src, nameSpace);            
            WriteSource(appData, src);

            WriteAssembly(appData, assembly, $"{nameSpace}.dll");

            return Assembly.Load(assembly);
        }

        private static void WriteAssembly(DirectoryInfo appData, byte[] assemblyFile, string assemblyFileName)
        {
            DirectoryInfo binDir = new DirectoryInfo(Path.Combine(appData.FullName, "_gen", "bin"));
            if (!binDir.Exists)
            {
                binDir.Create();
            }
            string dllFilePath = Path.Combine(binDir.FullName, assemblyFileName);
            File.WriteAllBytes(dllFilePath, assemblyFile);
        }

        private static void WriteSource(DirectoryInfo appData, string src)
        {
            DirectoryInfo srcDir = new DirectoryInfo(Path.Combine(appData.FullName, "_gen", "src"));
            if (!srcDir.Exists)
            {
                srcDir.Create();
            }
            FileInfo sourceFile = new FileInfo(Path.Combine(srcDir.FullName, $"{src.Sha256()}.cs"));
            src.SafeWriteToFile(sourceFile.FullName, true);
        }

        public DynamicTypeDescriptor AddType(string typeName)
        {
            return AddType(typeName, null);
        }

        public DynamicTypeDescriptor AddType(string typeName, string nameSpace)
        {
            return AddType(typeName, nameSpace, out DynamicNamespaceDescriptor ignore);
        }

        public DynamicTypeDescriptor AddType(string typeName, string nameSpace, out DynamicNamespaceDescriptor dynamicNamespaceDescriptor)
        {
            nameSpace = string.IsNullOrEmpty(nameSpace) ? DynamicNamespaceDescriptor.DefaultNamespace : nameSpace;
            return EnsureType(typeName, nameSpace, out dynamicNamespaceDescriptor);
        }

        public DynamicTypePropertyDescriptor AddProperty(string typeName, string propertyName, string propertyType, string nameSpace = null)
        {
            Type type = Type.GetType(propertyType);
            if(type == null)
            {
                type = Type.GetType($"System.{propertyType}");
            }
            Args.ThrowIfNull(type, "propertyType");
            nameSpace = nameSpace ?? DynamicNamespaceDescriptor.DefaultNamespace;
            DynamicNamespaceDescriptor nameSpaceDescriptor = EnsureNamespace(nameSpace);
            DynamicTypeDescriptor typeDescriptor = EnsureType(typeName, nameSpaceDescriptor);
            return SetDynamicTypePropertyDescriptor(new DynamicTypePropertyDescriptor
            {
                DynamicTypeDescriptorId = typeDescriptor.Id,
                ParentTypeName = typeDescriptor.TypeName,
                PropertyType = propertyType,
                PropertyName = propertyName
            });
        }

        [Verbosity(VerbosityLevel.Warning, SenderMessageFormat = "Multiple types found by the name of {TypeName}: {FoundTypes}")]
        public event EventHandler MultipleTypesFoundWarning;

        public DynamicTypeDescriptor GetTypeDescriptor(string typeName, string nameSpace = null)
        {
            List<DynamicTypeDescriptor> results = new List<DynamicTypeDescriptor>();
            if (string.IsNullOrWhiteSpace(nameSpace))
            {
                results = DynamicTypeDataRepository.DynamicTypeDescriptorsWhere(d => d.TypeName == typeName).ToList();
            }
            else
            {
                DynamicNamespaceDescriptor nspace = DynamicTypeDataRepository.DynamicNamespaceDescriptorsWhere(ns => ns.Namespace == nameSpace).FirstOrDefault();
                Args.ThrowIfNull(nspace, "nameSpace");
                results = DynamicTypeDataRepository.DynamicTypeDescriptorsWhere(d => d.TypeName == typeName && d.DynamicNamespaceDescriptorId == nspace.Id).ToList();
            }
            if (results.Count > 1)
            {
                FireEvent(MultipleTypesFoundWarning,
                    new DynamicTypeManagerEventArgs
                    {
                        DynamicTypeDescriptors = results.ToArray(),
                        TypeName = typeName,
                        FoundTypes = string.Join(", ", results.Select(dt => $"{dt.DynamicNamespaceDescriptor.Namespace}.{dt.TypeName}").ToArray())
                    });
            }
            return results.FirstOrDefault();
        }

        public DynamicNamespaceDescriptor GetNamespaceDescriptor(string nameSpaceName)
        {
            DynamicNamespaceDescriptor result = DynamicTypeDataRepository.GetOneDynamicNamespaceDescriptorWhere(d => d.Namespace == nameSpaceName);
            if(result != null)
            {
                result = DynamicTypeDataRepository.Retrieve<DynamicNamespaceDescriptor>(result.Id);
            }
            return result;
        }

        public Assembly GenerateAssembly()
        {
            string source = GenerateSource();
            return Assembly.Load(Compiler.Compile(source.Sha256(), source));
        }
        
        public byte[] GenerateAssembly(string nameSpace)
        {
            return GenerateAssembly(out string ignore, nameSpace);
        }

        public byte[] GenerateAssembly(out string source, string nameSpace = null)
        {
            source = GenerateSource(nameSpace, out DynamicNamespaceDescriptor ns);
            return Compiler.Compile(ns.Namespace, source);
        }

        public string GenerateSource()
        {
            StringBuilder src = new StringBuilder();
            DynamicTypeDataRepository.BatchAllDynamicTypeDescriptors(5, types => src.AppendLine(GenerateSource(types))).Wait();
            return src.ToString();
        }

        public string GenerateSource(string nameSpace)
        {
            return GenerateSource(nameSpace, out DynamicNamespaceDescriptor ignore);
        }

        public string GenerateSource(string nameSpace, out DynamicNamespaceDescriptor ns)
        {
            List<DynamicTypeDescriptor> types = new List<DynamicTypeDescriptor>();
            ns = null;
            if (!string.IsNullOrEmpty(nameSpace))
            {
                ns = GetNamespaceDescriptor(nameSpace);
            }
            else
            {
                ns = DynamicTypeDataRepository.GetOneDynamicNamespaceDescriptorWhere(d => d.Namespace == DynamicNamespaceDescriptor.DefaultNamespace);
            }
            ulong id = ns.Id;
            types = DynamicTypeDataRepository.DynamicTypeDescriptorsWhere(t => t.DynamicNamespaceDescriptorId == id).ToList();
            return GenerateSource(types);
        }

        public string GenerateSource(IEnumerable<DynamicTypeDescriptor> types)
        {
            StringBuilder src = new StringBuilder();
            foreach (DynamicTypeDescriptor typeDescriptor in types)
            {
                DtoModel dto = new DtoModel
                (
                    typeDescriptor.DynamicNamespaceDescriptor.Namespace,
                    GetClrTypeName(typeDescriptor.TypeName),
                    typeDescriptor.Properties.Select(p => new DtoPropertyModel { PropertyName = GetClrPropertyName(p.PropertyName), PropertyType = GetClrTypeName(p.PropertyType) }).ToArray()
                );
                src.AppendLine(dto.Render());
            }
            return src.ToString();
        }

        protected string GetClrTypeName(string jsonTypePath)
        {
            string typePath = jsonTypePath;
            string prefix = "arrayOf";
            bool isArray = false;
            if (typePath.StartsWith(prefix))
            {
                typePath = typePath.TruncateFront(prefix.Length).Truncate(1);
                isArray = true;
            }
            string[] splitOnDots = typePath.DelimitSplit(".");
            return splitOnDots[splitOnDots.Length - 1].PascalCase() + (isArray ? "[]": "");
        }

        protected string GetClrPropertyName(string jsonPropertyName)
        {
            return jsonPropertyName.PascalCase(true, new string[] { " ", "_", "-" });
        }

        /// <summary>
        /// Save a DynamicTypeDescriptor for the specified values
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="valueDictionary"></param>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        protected internal DynamicTypeDescriptor SaveTypeDescriptor(string typeName, Dictionary<object, object> valueDictionary, string nameSpace = null)
        {
            DynamicTypeDescriptor descriptor = EnsureType(typeName, nameSpace, out DynamicNamespaceDescriptor namespaceDescriptor);

            foreach (object key in valueDictionary.Keys)
            {
                object value = valueDictionary[key];
                if (value != null)
                {
                    Type childType = value.GetType();
                    string childTypeName = $"{typeName}.{key}";
                    DynamicTypePropertyDescriptor propertyDescriptor = new DynamicTypePropertyDescriptor
                    {
                        DynamicTypeDescriptorId = descriptor.Id,
                        ParentTypeName = descriptor.TypeName,
                        PropertyType = childType.Name,
                        PropertyName = key.ToString(),
                    };

                    if (childType == typeof(JObject) || childType == typeof(Dictionary<object, object>))
                    {
                        propertyDescriptor.PropertyType = childTypeName;
                        SetDynamicTypePropertyDescriptor(propertyDescriptor);
                        Dictionary<object, object> data = value as Dictionary<object, object>;
                        if(data is null)
                        {
                            data = ((JObject)value).ToObject<Dictionary<object, object>>();
                        }
                        SaveTypeDescriptor(childTypeName, data);
                    }
                    else if (childType == typeof(JArray) || childType.IsArray)
                    {
                        propertyDescriptor.PropertyType = $"arrayOf({childTypeName})";
                        SetDynamicTypePropertyDescriptor(propertyDescriptor);

                        foreach (object obj in (IEnumerable)value)
                        {
                            Dictionary<object, object> data = new Dictionary<object, object>();
                            if(obj is JObject jobj)
                            {
                                data = jobj.ToObject<Dictionary<object, object>>();
                                SaveTypeDescriptor(childTypeName, data);
                            }
                        }
                    }
                    else
                    {
                        SetDynamicTypePropertyDescriptor(propertyDescriptor);
                    }
                }
            }

            return DynamicTypeDataRepository.Retrieve<DynamicTypeDescriptor>(descriptor.Id);
        }
                
        object _typeDescriptorLock = new object();
        protected DynamicTypeDescriptor EnsureType(string typeName, string nameSpace, out DynamicNamespaceDescriptor namespaceDescriptor)
        {
            namespaceDescriptor = EnsureNamespace(nameSpace);

            return EnsureType(typeName, namespaceDescriptor);
        }

        protected DynamicTypeDescriptor EnsureType(string typeName, DynamicNamespaceDescriptor nspace)
        {
            lock (_typeDescriptorLock)
            {
                return DynamicTypeDataRepository.GetOneDynamicTypeDescriptorWhere(td => td.TypeName == typeName && td.DynamicNamespaceDescriptorId == nspace.Id);                
            }
        }

        object _nameSpaceLock = new object();
        protected DynamicNamespaceDescriptor EnsureNamespace(string nameSpace = null)
        {
            lock (_nameSpaceLock)
            {
                nameSpace = nameSpace ?? DynamicNamespaceDescriptor.DefaultNamespace;
                return DynamicTypeDataRepository.GetOneDynamicNamespaceDescriptorWhere(ns => ns.Namespace == nameSpace);
            }
        }

        static Dictionary<int, object> _dynamicTypePropertyLocks = new Dictionary<int, object>();
        private DynamicTypePropertyDescriptor SetDynamicTypePropertyDescriptor(DynamicTypePropertyDescriptor prop)
        {
            int hashCode = prop.GetHashCode();
            _dynamicTypePropertyLocks.AddMissing(hashCode, new object());
            lock (_dynamicTypePropertyLocks[hashCode])
            {
                DynamicTypePropertyDescriptor retrieved = DynamicTypeDataRepository.DynamicTypePropertyDescriptorsWhere(pd =>
                    pd.DynamicTypeDescriptorId == prop.DynamicTypeDescriptorId &&
                    pd.ParentTypeName == prop.ParentTypeName &&
                    pd.PropertyType == prop.PropertyType &&
                    pd.PropertyName == prop.PropertyName).FirstOrDefault();

                if (retrieved == null)
                {
                    retrieved = DynamicTypeDataRepository.Save(prop);
                }
                return retrieved;
            }
        }

        protected internal DynamicTypeDescriptor GetDynamicTypeDescriptor(string partialOrFullyQualifiedTypeName, out DynamicNamespaceDescriptor namespaceDescriptor)
        {
            string typeName = partialOrFullyQualifiedTypeName;
            if (partialOrFullyQualifiedTypeName.Contains("."))
            {
                typeName = partialOrFullyQualifiedTypeName.Substring(partialOrFullyQualifiedTypeName.LastIndexOf("."));
                string nameSpace = partialOrFullyQualifiedTypeName.Substring(0, partialOrFullyQualifiedTypeName.LastIndexOf("."));
                namespaceDescriptor = EnsureNamespace(nameSpace);
                return DynamicTypeDataRepository.GetOneDynamicTypeDescriptorWhere(t => t.TypeName == typeName);
            }
            else
            {
                DynamicTypeDescriptor typeDescriptor = DynamicTypeDataRepository.GetOneDynamicTypeDescriptorWhere(t => t.TypeName == typeName);
                if(typeDescriptor.DynamicNamespaceDescriptorId > 0)
                {
                    namespaceDescriptor = DynamicTypeDataRepository.GetOneDynamicNamespaceDescriptorWhere(ns => ns.Id == typeDescriptor.DynamicNamespaceDescriptorId);
                }
                else
                {
                    namespaceDescriptor = DynamicNamespaceDescriptor.GetDefault(DynamicTypeDataRepository);
                }
                return typeDescriptor;
            }
        }
        protected void SetReferenceAssemblies()
        {
            List<Assembly> assemblies = new List<Assembly>(Compiler.AssembliesToReference)
            {
                typeof(ActionResult).Assembly
            };
            Compiler.AssembliesToReference = assemblies.ToArray();
        }
    }
}
