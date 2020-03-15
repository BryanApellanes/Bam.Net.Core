using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bam.Net.Data.Schema;
using Bam.Net.Logging;
using Bam.Net.Schema.Json;
using CsQuery.ExtensionMethods.Internal;
using Markdig.Helpers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Bam.Net.Application.Json
{
    public class JSchemaClassManager 
    {
        private readonly List<string> _classNameProperties;
        private readonly Dictionary<string, Func<string, string>> _classNamePropertyMungers;

        public JSchemaClassManager(JSchemaResolver jSchemaResolver):this("className")
        {
            JSchemaResolver = jSchemaResolver;
            Logger = Log.Default;
        }

        /// <summary>
        /// Instantiate a new JSchemaManager.
        /// </summary>
        /// <param name="classNameProperties">The property names to look for the class name in.  JsonSchema doesn't specify where or how to declare
        /// the strongly typed name for a given schema, so we must specify that as part of our custom/proprietary implementation.</param>
        public JSchemaClassManager(params string[] classNameProperties)
        {
            JSchemaNameParser = new JSchemaNameParser();
            _classNameProperties = new List<string>(classNameProperties);
            _classNamePropertyMungers = new Dictionary<string, Func<string, string>>();
            SetClassNameExtractor();
        }

        public JSchemaClassManager(Dictionary<string, Func<string, string>> classNamePropertyMungers)
        {
            JSchemaNameParser = new JSchemaNameParser();
            _classNameProperties = new List<string>(classNamePropertyMungers.Keys.ToArray());
            _classNamePropertyMungers = classNamePropertyMungers;
            SetClassNameExtractor();
        }
        
        protected string ExtractClassNameFromClassNameProperties(JSchema jSchema)
        {
            foreach (string classNameProperty in _classNameProperties)
            {
                if (jSchema.HasProperty(classNameProperty, out object value))
                {
                    if (_classNamePropertyMungers.ContainsKey(classNameProperty))
                    {
                        return _classNamePropertyMungers[classNameProperty](value.ToString());
                    }
                    else
                    {
                        return MungeClassName == null ? value.ToString(): MungeClassName(value.ToString());
                    }
                }
            }

            return string.Empty;
        }
        
        public JSchemaResolver JSchemaResolver { get; set; }
        public JSchemaNameParser JSchemaNameParser { get; set; }
        
        public ILogger Logger { get; set; }
        /// <summary>
        /// A function used to further parse a class name when it is found.  This is intended
        /// to apply any conventions to the name that are not enforced in the JSchema.  Parse the
        /// inbound string and return an appropriate class name based on it.
        /// </summary>
        public Func<string, string> MungeClassName
        {
            get => JSchemaNameParser.MungeClassName;
            set => JSchemaNameParser.MungeClassName = value;
        }

        public Func<JSchema, string> ExtractJSchemaClassName
        {
            get => JSchemaNameParser.ExtractJSchemaClassName;
            set => JSchemaNameParser.ExtractJSchemaClassName = value;
        }

        public Func<JObject, string> ExtractJObjectClassName
        {
            get => JSchemaNameParser.ExtractJObjectClassName;
            set => JSchemaNameParser.ExtractJObjectClassName = value;
        }

        public Func<string, string> MungeEnumName
        {
            get => JSchemaNameParser.MungeEnumName;
            set => JSchemaNameParser.MungeEnumName = value;
        }

        /// <summary>
        /// A function used to further parse a property name when it is found.  This is intended to
        /// apply any conventions to the name that are not enforced in the JSchema.  Parse the
        /// inbound string and return an appropriate property name based on it.
        /// </summary>
        public Func<string, string> ParsePropertyName
        {
            get => JSchemaNameParser.ParsePropertyName;
            set => JSchemaNameParser.ParsePropertyName = value;
        }

        public JSchemaClassManager SetClassNameMunger(string classNameProperty, Func<string, string> munger)
        {
            if (_classNamePropertyMungers.ContainsKey(classNameProperty))
            {
                Logger.Warning("Replacing class name munger for class name property :{0}", classNameProperty);
                _classNamePropertyMungers[classNameProperty] = munger;
            }
            else
            {
                _classNamePropertyMungers.Add(classNameProperty, munger);
            }

            return this;
        }
        
        public HashSet<JSchemaClass> LoadJSchemaClasses(string directoryPath)
        {
            return LoadJSchemaClasses(new DirectoryInfo(directoryPath));
        }
        
        public HashSet<JSchemaClass> LoadJSchemaClasses(DirectoryInfo directoryInfo)
        {
            HashSet<JSchemaClass> results = new HashSet<JSchemaClass>();
            foreach (JSchemaClass jSchemaClass in LoadJSchemaClassFiles(directoryInfo))
            {
                if (!jSchemaClass.Properties.Any())
                {
                    continue;
                }
                results.Add(jSchemaClass);
                foreach (JSchemaProperty property in jSchemaClass.Properties)
                {
                    if (property.ClassOfArrayItems != null)
                    {
                        JSchemaClass classOfArrayItems = property.ClassOfArrayItems;
                        if (classOfArrayItems.Properties.Any())
                        {
                            if (string.IsNullOrEmpty(classOfArrayItems.ClassName))
                            {
                                classOfArrayItems.ClassName = property.PropertyName;
                            }
                            results.Add(classOfArrayItems);
                        }
                    }

                    if (property.ClassOfProperty != null)
                    {
                        JSchemaClass classOfProperty = property.ClassOfProperty;
                        if (classOfProperty.Properties.Any())
                        {
                            if (string.IsNullOrEmpty(classOfProperty.ClassName))
                            {
                                classOfProperty.ClassName = property.PropertyName;
                            }
                            results.Add(classOfProperty);
                        }
                    }
                }
            }

            return results;
        }
        
        public IEnumerable<JSchemaClass> LoadJSchemaClassFiles(DirectoryInfo directoryInfo)
        {
            Args.ThrowIfNull(directoryInfo, "directoryInfo");

            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                yield return LoadJSchemaClassFile(fileInfo);
            }
        }
        
        public JSchemaClass LoadJSchemaClassFile(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            return LoadJSchemaClassFile(fileInfo);
        }

        public JSchemaClass LoadJSchemaClassFile(FileInfo fileInfo)
        {
            JSchemaLoader loader = GetJSchemaLoader(fileInfo);
            if (loader == null)
            {
                string msgFormat = "No loader for file {0}";
                Logger.Warning(msgFormat, fileInfo.FullName);
                return new JSchemaClass(this, string.Format(msgFormat, fileInfo.FullName));
            }

            loader.JSchemaResolver = JSchemaResolver;
            JSchema result = loader.LoadSchema(fileInfo.FullName);
            JSchemaClass resultClass = new JSchemaClass(result, this, fileInfo.FullName);
            return resultClass;
        }

        private static JSchemaLoader GetJSchemaLoader(FileInfo fileInfo)
        {
            JSchemaLoader loader = null;
            if (fileInfo.Extension.Equals(".yaml", StringComparison.InvariantCultureIgnoreCase))
            {
                loader = JSchemaLoader.ForFormat(SerializationFormat.Yaml);
            }
            else if (fileInfo.Extension.Equals(".json", StringComparison.InvariantCultureIgnoreCase))
            {
                loader = JSchemaLoader.ForFormat(SerializationFormat.Json);
            }

            return loader;
        }
        
        private void SetClassNameExtractor()
        {
            Func<JSchema, string> defaultClassNameExtractor = JSchemaNameParser.ExtractJSchemaClassName;
            JSchemaNameParser.ExtractJSchemaClassName = jSchema =>
            {
                string defaultClassName = defaultClassNameExtractor(jSchema);
                string fromClassNameProperties = ExtractClassNameFromClassNameProperties(jSchema);
                if (!defaultClassName.Equals(fromClassNameProperties))
                {
                    return fromClassNameProperties;
                }

                return defaultClassName;
            };
        }
    }
}