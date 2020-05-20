using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Bam.Net.CoreServices.AssemblyManagement;
using Bam.Net.Data.Schema;
using Bam.Net.Data.Schema.Handlebars;
using Bam.Net.Logging;
using Bam.Net.Services;
using Newtonsoft.Json;

namespace Bam.Net.Data.Repositories.Handlebars
{
    public class HandlebarsTypeDaoGenerator: TypeDaoGenerator
    {
        public HandlebarsTypeDaoGenerator(TypeSchemaGenerator typeSchemaGenerator, ILogger logger = null) : base(new HandlebarsDaoCodeWriter(), new DaoTargetStreamResolver())
        {
            TypeSchemaGenerator = typeSchemaGenerator;
            WrapperGenerator = new HandlebarsWrapperGenerator(WrapperNamespace, DaoNamespace);
        }

        [Inject]
        public IReferenceAssemblyResolver ReferenceAssemblyResolver { get; set; }

        protected internal override bool GenerateDaoAssembly(TypeSchema typeSchema, out CompilationException compilationEx)
        {
            compilationEx = null;
            try
            {
                SchemaDefinition schema = SchemaDefinitionCreateResult.SchemaDefinition;
                string assemblyName = $"{schema.Name}.dll";

                string writeSourceTo = TypeSchemaTempPathProvider(schema, typeSchema);
                TryDeleteDaoTemp(writeSourceTo);
                GenerateSource(writeSourceTo);
                byte[] assembly = Compile(assemblyName, writeSourceTo);
                GeneratedDaoAssemblyInfo info =
                    new GeneratedDaoAssemblyInfo(schema.Name, Assembly.Load(assembly), assembly)
                    {
                        TypeSchema = typeSchema,
                        SchemaDefinition = schema
                    };

                info.Save();

                GeneratedAssemblies.SetAssemblyInfo(schema.Name, info);

                Message = "Type Dao Generation completed successfully";
                FireGenerateDaoAssemblySucceeded(new GenerateDaoAssemblyEventArgs(info));

                TryDeleteDaoTemp(writeSourceTo);

                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    Message = "{0}:\r\nStackTrace: {1}"._Format(Message, ex.StackTrace);
                }

                FireGenerateDaoAssemblyFailed(ex);
                return false;
            }
        }

        protected override HashSet<string> GetDefaultReferenceAssemblies()
        {
            IReferenceAssemblyResolver referenceAssemblyResolver = ReferenceAssemblyResolver ?? CoreServices.AssemblyManagement.ReferenceAssemblyResolver.Current;
            
            HashSet<string> result = new HashSet<string>()
            {
                typeof(JsonConvert).Assembly.GetFilePath(),
                typeof(MarshalByValueComponent).Assembly.GetFilePath(),
                typeof(Enumerable).Assembly.GetFilePath(),
                typeof(Object).Assembly.GetFilePath(),
                referenceAssemblyResolver.ResolveReferenceAssemblyPath("System.Collections.dll"),
                referenceAssemblyResolver.ResolveReferenceAssemblyPath("netstandard.dll"),
                typeof(Attribute).Assembly.GetFilePath(),
                referenceAssemblyResolver.ResolveSystemRuntimePath()
            };
            return result;
        }

        private byte[] Compile(string assemblyNameToCreate, string sourcePath)
        {
            HashSet<string> references = GetReferenceAssemblies();
            RoslynCompiler compiler = new RoslynCompiler();
            references.Each(path => compiler.AddAssemblyReference(path));
            return compiler.Compile(assemblyNameToCreate, new DirectoryInfo(sourcePath));
        }
    }
}
