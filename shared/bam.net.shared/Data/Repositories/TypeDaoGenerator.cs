/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Bam.Net.Analytics;
using Bam.Net.CommandLine;
using Bam.Net.Configuration;
using Bam.Net.CoreServices.AssemblyManagement;
using Bam.Net.Data.Schema;
using Bam.Net.Logging;
using Bam.Net.Services;
using Newtonsoft.Json;

namespace Bam.Net.Data.Repositories
{
    /// <summary>
    /// A class used to generate data access objects from
    /// CLR types.
    /// </summary>
    [Serializable]
    public class TypeDaoGenerator : Loggable, IGeneratesDaoAssembly, IHasTypeSchemaTempPathProvider, ISourceGenerator
    {
        DaoGenerator _daoGenerator;
        IWrapperGenerator _wrapperGenerator;
        TypeSchemaGenerator _typeSchemaGenerator;
        HashSet<Assembly> _additionalReferenceAssemblies;
        HashSet<Type> _additionalReferenceTypes;

        public TypeDaoGenerator(IDaoCodeWriter codeWriter, IDaoTargetStreamResolver targetStreamResolver)
        {
            _namespace = "TypeDaos";
            _daoGenerator = new DaoGenerator(codeWriter, targetStreamResolver) { Namespace = DaoNamespace };
            _wrapperGenerator = new RazorWrapperGenerator(WrapperNamespace, DaoNamespace);
            _typeSchemaGenerator = new TypeSchemaGenerator();
            _types = new HashSet<Type>();
            _additionalReferenceAssemblies = new HashSet<Assembly>();
            _additionalReferenceTypes = new HashSet<Type>();
            
            SetTempPathProvider();
            SubscribeToSchemaWarnings();
        }

        /// <summary>
        /// Instantiate a new instance of TypeDaoGenerator
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="types"></param>
        public TypeDaoGenerator(ILogger logger = null, IEnumerable<Type> types = null)
        {
            _namespace = "TypeDaos";
            _daoGenerator = new DaoGenerator(DaoNamespace);
            _wrapperGenerator = new RazorWrapperGenerator(WrapperNamespace, DaoNamespace);
            _typeSchemaGenerator = new TypeSchemaGenerator();
            _additionalReferenceAssemblies = new HashSet<Assembly>();
            _additionalReferenceTypes = new HashSet<Type>();

            SetTempPathProvider();
            SubscribeToSchemaWarnings();
            _types = new HashSet<Type>();
            if (logger != null)
            {
                Subscribe(logger);
            }
            if(types != null)
            {
                AddTypes(types);
            }
        }
        
        /// <summary>
        /// Instantiate a new instance of TypeDaoGenerator
        /// </summary>
        /// <param name="typeAssembly"></param>
        /// <param name="nameSpace"></param>
        /// <param name="logger"></param>
        public TypeDaoGenerator(Assembly typeAssembly, string nameSpace, ILogger logger = null)
            : this(logger)
        {
            BaseNamespace = nameSpace;
            Args.ThrowIfNull(typeAssembly, "typeAssembly");
            AddTypes(typeAssembly.GetTypes().Where(t => t.Namespace != null && t.Namespace.Equals(nameSpace)));
        }
        
        public TypeDaoGenerator(TypeSchemaGenerator typeSchemaGenerator) : this()
        {
            _typeSchemaGenerator = typeSchemaGenerator;
        }

        [Inject]
        public DaoGenerator DaoGenerator
        {
            get => _daoGenerator;
            set => _daoGenerator = value;
        }

        [Inject]
        public IWrapperGenerator WrapperGenerator
        {
            get => _wrapperGenerator;
            set => _wrapperGenerator = value;
        }

        protected TypeSchemaGenerator TypeSchemaGenerator
        {
            get => _typeSchemaGenerator;
            set => _typeSchemaGenerator = value;
        }

        /// <summary>
        /// A filter function used to exclude anonymous types
        /// that were created by the use of lambda functions from 
        /// having dao types generated
        /// </summary>
        public static Func<Type, bool> ClrDaoTypeFilter
        {
            get
            {
                return (t) => !t.IsAbstract && !t.HasCustomAttributeOfType<CompilerGeneratedAttribute>()
                && t.Attributes != (
                        TypeAttributes.NestedPrivate |
                        TypeAttributes.Sealed |
                        TypeAttributes.Serializable |
                        TypeAttributes.BeforeFieldInit
                    );
            }
        }

        public bool IncludeModifiedBy { get; set; }

        public bool CheckIdField { get; set; }

        string _namespace;

        /// <summary>
        /// The namespace containing POCO types to generate dao types for.  Setting 
        /// the TargetNamespace will also set the DaoNamespace
        /// and WrapperNamespace. This is the 
        /// same as BaseNamespace and exists for contextual intuitiveness.
        /// </summary>
        /// <value>
        /// The target namespace.
        /// </value>
        public string TargetNamespace
        {
            get => BaseNamespace;
            set => BaseNamespace = value;
        }

        /// <summary>
        /// The namespace containing POCO types to generate dao types for.  Setting 
        /// the BaseNamespace will also set the DaoNamespace
        /// and WrapperNamespace.
        /// </summary>
        public string BaseNamespace
        {
            get => _namespace;
            set
            {
                _namespace = value;
                _daoGenerator.Namespace = DaoNamespace;
                _wrapperGenerator.WrapperNamespace = WrapperNamespace;
                _wrapperGenerator.DaoNamespace = DaoNamespace;
            }
        }

        string _daoNamespace;
        public string DaoNamespace
        {
            get => _daoNamespace ?? $"{_namespace}.Dao";
            set
            {
                _daoNamespace = value;
                _daoGenerator.Namespace = _daoNamespace;
                _wrapperGenerator.DaoNamespace = _daoNamespace;
            }
        }

        string _wrapperNamespace;
        public string WrapperNamespace
        {
            get => _wrapperNamespace ?? $"{_namespace}.Wrappers";
            set
            {
                _wrapperNamespace = value;
                _wrapperGenerator.WrapperNamespace = _wrapperNamespace;                
            }
        }

        string _schemaName;
        public string SchemaName
        {
            get
            {
                if (string.IsNullOrEmpty(_schemaName))
                {
                    _schemaName = $"_{_types.ToInfoHash()}_Dao";
                }

                return _schemaName;
            }
            set => _schemaName = value;
        }

        public override void Subscribe(ILogger logger)
        {
            _typeSchemaGenerator.Subscribe(logger);
            base.Subscribe(logger);
        }

        HashSet<Type> _types;
        public Type[] Types => _types.ToArray();

        public bool KeepSource { get; set; }

        public void AddTypes(IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                AddType(type);
            }
        }

        public void AddType(Type type)
        {
            if (!ClrDaoTypeFilter(type))
                return;

            if (type.GetProperty("Id") == null &&
                !type.HasCustomAttributeOfType<KeyAttribute>() &&
                CheckIdField)
            {
                throw new NoIdPropertyException(type);
            }

            if (type.HasEnumerableOfMe(type))
            {
                throw new NotSupportedException("Storable types cannot have enumerable properties that are of the same type as themselves.");
            }

            AddAdditionalReferenceAssemblies(new TypeInheritanceDescriptor(type));
            CustomAttributeTypeDescriptor attrs = new CustomAttributeTypeDescriptor(type);
            attrs.AttributeTypes.Each(attrType => AddAdditionalReferenceAssemblies(new TypeInheritanceDescriptor(attrType)));
            _types.Add(type);
        }

        public void AddReferenceAssembly(Assembly assembly)
        {
            _additionalReferenceAssemblies.Add(assembly);
        }

        private void AddAdditionalReferenceAssemblies(TypeInheritanceDescriptor typeInheritanceDescriptor)
        {
            if (!_additionalReferenceTypes.Contains(typeInheritanceDescriptor.Type))
            {
                AddAdditionalReferenceAssemblies(typeInheritanceDescriptor.Type);
                foreach (TypeTable type in typeInheritanceDescriptor.Chain)
                {
                    AddAdditionalReferenceAssemblies(type.Type);
                }
            }
        }
        private void AddAdditionalReferenceAssemblies(Type type)
        {
            if (!_additionalReferenceTypes.Contains(type))
            {
                _additionalReferenceAssemblies.Add(type.Assembly);
                foreach (MethodInfo method in type.GetMethods())
                {
                    _additionalReferenceAssemblies.Add(method.ReturnType.Assembly);
                    foreach (System.Reflection.ParameterInfo parameter in method.GetParameters())
                    {
                        _additionalReferenceAssemblies.Add(parameter.ParameterType.Assembly);
                    }
                }
                foreach (PropertyInfo property in type.GetProperties())
                {
                    _additionalReferenceAssemblies.Add(property.PropertyType.Assembly);
                }
                foreach (ConstructorInfo ctor in type.GetConstructors())
                {
                    foreach (System.Reflection.ParameterInfo parameter in ctor.GetParameters())
                    {
                        _additionalReferenceAssemblies.Add(parameter.ParameterType.Assembly);
                    }
                }
            }
        }

        public Assembly GetDaoAssembly(bool useExisting = true)
        {
            GeneratedDaoAssemblyInfo info = GeneratedAssemblies.GetGeneratedAssemblyInfo(SchemaName) as GeneratedDaoAssemblyInfo;
            if (info == null)
            {
                TypeSchema typeSchema = SchemaDefinitionCreateResult.TypeSchema;
                SchemaDefinition schemaDef = SchemaDefinitionCreateResult.SchemaDefinition;
                string schemaName = schemaDef.Name;
                string schemaHash = typeSchema.Hash;
                info = new GeneratedDaoAssemblyInfo(schemaName, typeSchema, schemaDef);

                // check for the info file
                if (info.InfoFileExists && useExisting) // load it from file if it exists
                {
                    info = info.InfoFilePath.FromJsonFile<GeneratedDaoAssemblyInfo>();
                    if (info.TypeSchemaHash == null || !info.TypeSchemaHash.Equals(schemaHash)) // regenerate if the hashes don't match
                    {
                        ReportDiff(info, typeSchema);
                        GenerateOrThrow(schemaDef, typeSchema);
                    }
                    else
                    {
                        GeneratedAssemblies.SetAssemblyInfo(schemaName, info);
                    }
                }
                else
                {
                    GenerateOrThrow(schemaDef, typeSchema);
                }

                info = GeneratedAssemblies.GetGeneratedAssemblyInfo(SchemaName) as GeneratedDaoAssemblyInfo;
            }

            return info.GetAssembly();
        }

        SchemaDefinitionCreateResult _schemaDefinitionCreateResult;
        object _schemaDefinitionCreateResultLock = new object();
        public SchemaDefinitionCreateResult SchemaDefinitionCreateResult
        {
            get
            {
                return _schemaDefinitionCreateResultLock.DoubleCheckLock(ref _schemaDefinitionCreateResult, () => CreateSchemaDefinition(SchemaName));
            }
        }

        [Verbosity(VerbosityLevel.Error, SenderMessageFormat = "Failed to generate DaoAssembly for {SchemaName}:\r\n {Message}")]
        public event EventHandler GenerateDaoAssemblyFailed;

        [Verbosity(VerbosityLevel.Information, SenderMessageFormat = "{Message}")]
        public event EventHandler GenerateDaoAssemblySucceeded;

        public string TempPath { get; set; }

        public string Message { get; set; }

        [Verbosity(VerbosityLevel.Warning, SenderMessageFormat = "Couldn't delete folder {TempPath}:\r\nMessage: {Message}")]
        public event EventHandler DeleteDaoTempFailed;

        public Func<SchemaDefinition, TypeSchema, string> TypeSchemaTempPathProvider { get; set; }

        [Verbosity(VerbosityLevel.Warning, SenderMessageFormat = "TypeSchema difference detected\r\n {OldInfoString} \r\n *** \r\n {NewInfoString}")]
        public event EventHandler SchemaDifferenceDetected;
        public string OldInfoString { get; set; }
        public string NewInfoString { get; set; }

        /// <summary>
        /// Warnings related to the type definitions, see TypeSchemaWarnings enum for possible warnings.
        /// </summary>
        public HashSet<TypeSchemaWarning> TypeSchemaWarnings => SchemaDefinitionCreateResult.TypeSchemaWarnings;
        public bool MissingColumns => SchemaDefinitionCreateResult.MissingColumns;
        public SchemaWarnings Warnings => SchemaDefinitionCreateResult.Warnings;

        public bool WarningsAsErrors
        {
            get; set;
        }

        [Verbosity(VerbosityLevel.Warning, EventArgsMessageFormat = "{Warning}: ParentType={ParentType}, ForeignKeyType={ForeignKeyType}")]
        public event EventHandler TypeSchemaWarning;

        [Verbosity(VerbosityLevel.Warning, SenderMessageFormat = "Missing {PropertyType} property: {ClassName}.{PropertyName}")]
        public event EventHandler SchemaWarning;
        
        protected internal void EmitWarnings()
        {
            if (MissingColumns)
            {
                if (this.Warnings.MissingForeignKeyColumns.Length > 0)
                {
                    foreach (ForeignKeyColumn fk in this.Warnings.MissingForeignKeyColumns)
                    {
                        DaoRepositorySchemaWarningEventArgs drswea = GetEventArgs(fk);
                        FireEvent(SchemaWarning, drswea);
                    }
                }
                if (this.Warnings.MissingKeyColumns.Length > 0)
                {
                    foreach (KeyColumn keyColumn in this.Warnings.MissingKeyColumns)
                    {
                        DaoRepositorySchemaWarningEventArgs drswea = GetEventArgs(keyColumn);
                        FireEvent(SchemaWarning, drswea);
                    }
                }
            }

            if (TypeSchemaWarnings.Count > 0)
            {
                foreach (TypeSchemaWarning warning in TypeSchemaWarnings)
                {
                    FireEvent(TypeSchemaWarning, warning.ToEventArgs());
                }
            }
        }

        public void ThrowWarningsIfWarningsAsErrors()
        {
            if (MissingColumns && WarningsAsErrors)
            {
                if (this.Warnings.MissingForeignKeyColumns.Length > 0)
                {
                    List<string> missingColumns = new List<string>();
                    foreach (ForeignKeyColumn fk in this.Warnings.MissingForeignKeyColumns)
                    {
                        DaoRepositorySchemaWarningEventArgs drswea = GetEventArgs(fk);
                        missingColumns.Add("{ClassName}.{PropertyName}".NamedFormat(drswea));
                    }
                    throw new MissingForeignKeyPropertyException(missingColumns);
                }
                if (this.Warnings.MissingKeyColumns.Length > 0)
                {
                    List<string> classNames = new List<string>();
                    foreach (KeyColumn k in this.Warnings.MissingKeyColumns)
                    {
                        DaoRepositorySchemaWarningEventArgs drswea = GetEventArgs(k);
                        classNames.Add(k.TableClassName);
                    }
                    throw new NoIdPropertyException(classNames);
                }
            }

            if (TypeSchemaWarnings.Count > 0 && WarningsAsErrors)
            {
                throw new TypeSchemaException(TypeSchemaWarnings.ToArray());
            }
        }

        /// <summary>
        /// Create a SchemaDefinitionCreateResult for the types currently
        /// added to the TypeDaoGenerator
        /// </summary>
        /// <param name="schemaName"></param>
        /// <returns></returns>
        protected internal SchemaDefinitionCreateResult CreateSchemaDefinition(string schemaName = null)
        {
            return _typeSchemaGenerator.CreateSchemaDefinition(_types, schemaName);
        }

        protected internal virtual bool GenerateDaoAssembly(TypeSchema typeSchema, out CompilationException compilationEx)
        {
            try
            {
                compilationEx = null;
                SchemaDefinition schema = SchemaDefinitionCreateResult.SchemaDefinition;
                string assemblyName = $"{schema.Name}.dll";

                string writeSourceTo = TypeSchemaTempPathProvider(schema, typeSchema);
                CompilerResults results = GenerateAndCompile(assemblyName, writeSourceTo);
                GeneratedDaoAssemblyInfo info = new GeneratedDaoAssemblyInfo(schema.Name, results)
                {
                    TypeSchema = typeSchema,
                    SchemaDefinition = schema
                };
                info.Save();

                GeneratedAssemblies.SetAssemblyInfo(schema.Name, info);

                Message = "Type Dao Generation completed successfully";
                FireEvent(GenerateDaoAssemblySucceeded, new GenerateDaoAssemblyEventArgs(info));

                TryDeleteDaoTemp(writeSourceTo);

                return true;
            }
            catch (CompilationException ex)
            {
                Message = ex.Message;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    Message = "{0}:\r\nStackTrace: {1}"._Format(Message, ex.StackTrace);
                }
                compilationEx = ex;
                FireGenerateDaoAssemblyFailed(ex);
                return false;
            }
        }

        protected void FireGenerateDaoAssemblySucceeded(GenerateDaoAssemblyEventArgs args)
        {
            FireEvent(GenerateDaoAssemblySucceeded, args);
        }
        
        protected void FireGenerateDaoAssemblyFailed(Exception ex = null)
        {
            FireEvent(GenerateDaoAssemblyFailed, new GenerateDaoAssemblyEventArgs(ex));
        }
        
        protected internal CompilerResults GenerateAndCompile(string assemblyNameToCreate, string writeSourceTo)
        {
            TryDeleteDaoTemp(writeSourceTo);
            GenerateSource(writeSourceTo);

            return Compile(assemblyNameToCreate, writeSourceTo);
        }

        /// <summary>
        /// Generate source code for the current set of types
        /// </summary>
        /// <param name="writeSourceTo"></param>
        public virtual void GenerateSource(string writeSourceTo)
        {
            EmitWarnings();
            ThrowWarningsIfWarningsAsErrors();
            GenerateDaos(SchemaDefinitionCreateResult.SchemaDefinition, writeSourceTo);
            GenerateWrappers(SchemaDefinitionCreateResult.TypeSchema, writeSourceTo);
        }

        protected internal void GenerateDaos(SchemaDefinition schema, string writeSourceTo)
        {
            _daoGenerator.Generate(schema, writeSourceTo);
        }

        protected internal void GenerateWrappers(TypeSchema schema, string writeSourceTo)
        {
            _wrapperGenerator.Generate(schema, writeSourceTo);
        }

        protected internal CompilerResults Compile(string assemblyNameToCreate, string writeSourceTo)
        {
            HashSet<string> references = GetReferenceAssemblies();
            CompilerResults results = AdHocCSharpCompiler.CompileDirectories(new DirectoryInfo[] { new DirectoryInfo(writeSourceTo) }, assemblyNameToCreate, references.ToArray(), false);

            return results;
        }

        protected HashSet<string> GetReferenceAssemblies()
        {
            HashSet<string> references = GetDefaultReferenceAssemblies();
            _additionalReferenceAssemblies.Each(asm =>
            {
                FileInfo assemblyInfo = asm.GetFileInfo();
                if (references.Contains(assemblyInfo.Name))
                {
                    references.Remove(assemblyInfo.Name); // removes System.Core.dll if it is later referenced by full path
                }

                references.Add(assemblyInfo.FullName);
            });
            SchemaDefinitionCreateResult.TypeSchema.Tables.Each(type =>
            {
                references.Add(type.Assembly.GetFileInfo().FullName);
                CustomAttributeTypeDescriptor attrTypes = new CustomAttributeTypeDescriptor(type);
                attrTypes.AttributeTypes.Each(attrType => references.Add(attrType.Assembly.GetFileInfo().FullName));
            });
            references.Add(typeof(DaoRepository).Assembly.GetFileInfo().FullName);
            return references;
        }

        protected virtual HashSet<string> GetDefaultReferenceAssemblies()
        {
            HashSet<string> references = new HashSet<string>(DaoGenerator.DefaultReferenceAssemblies.ToArray())
            {
                typeof(JsonIgnoreAttribute).Assembly.GetFileInfo().FullName
            };
            return references;
        }

        private static DaoRepositorySchemaWarningEventArgs GetEventArgs(KeyColumn keyColumn)
        {
            string className = keyColumn.TableClassName;
            DaoRepositorySchemaWarningEventArgs drswea = new DaoRepositorySchemaWarningEventArgs { ClassName = className, PropertyName = "Id", PropertyType = "key column" };
            return drswea;
        }
        private static DaoRepositorySchemaWarningEventArgs GetEventArgs(ForeignKeyColumn fk)
        {
            string referencingClassName = fk.ReferencingClass.EndsWith("Dao") ? fk.ReferencingClass.Truncate(3) : fk.ReferencingClass;
            string propertyName = fk.PropertyName;
            DaoRepositorySchemaWarningEventArgs drswea = new DaoRepositorySchemaWarningEventArgs { ClassName = referencingClassName, PropertyName = propertyName, PropertyType = "foreign key" };
            return drswea;
        }
        private void GenerateOrThrow(SchemaDefinition schema, TypeSchema typeSchema)
        {
            string tempPath = TypeSchemaTempPathProvider(schema, typeSchema);
            if (Directory.Exists(tempPath))
            {
                string newPath = tempPath.GetNextDirectoryName();
                Directory.Move(tempPath, newPath);
            }
            if (!GenerateDaoAssembly(typeSchema, out CompilationException compilationException))
            {
                throw new DaoGenerationException(SchemaName, typeSchema.Hash, Types.ToArray(), compilationException);
            }
        }

        protected internal bool TryDeleteDaoTemp(string writeSourceTo)
        {
            if (!KeepSource)
            {
                try
                {
                    TempPath = writeSourceTo;
                    if (Directory.Exists(writeSourceTo))
                    {
                        Directory.Delete(writeSourceTo, true);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                    if (!string.IsNullOrEmpty(ex.StackTrace))
                    {
                        Message = $"{Message}\r\nStackTrace: {ex.StackTrace}";
                    }
                    FireEvent(DeleteDaoTempFailed, EventArgs.Empty);
                    return false;
                }
            }
            return false;
        }

        private void ReportDiff(GeneratedDaoAssemblyInfo info, TypeSchema typeSchema)
        {
            OldInfoString = info.TypeSchemaInfo ?? string.Empty;
            NewInfoString = typeSchema.ToString();
            DiffReport diff = DiffReport.Create(OldInfoString, NewInfoString);
            // TODO: inject this and log
            ConsoleDiffReportFormatter diffFormatter = new ConsoleDiffReportFormatter(diff);
            diffFormatter.Format(); // outputs to console
            FireEvent(SchemaDifferenceDetected, new SchemaDifferenceEventArgs { GeneratedDaoAssemblyInfo = info, TypeSchema = typeSchema, DiffReport = diff });
        }

        private void SubscribeToSchemaWarnings()
        {
            _typeSchemaGenerator.Subscribe(VerbosityLevel.Warning, (l, a) =>
            {
                FireEvent(TypeSchemaWarning, l, a);
            });
        }
        
        private void SetTempPathProvider()
        {
            TypeSchemaTempPathProvider = (schemaDef, typeSchema) =>
                System.IO.Path.Combine(RuntimeSettings.ProcessDataFolder, "DaoTemp_{0}"._Format(schemaDef.Name));
        }
    }
}
