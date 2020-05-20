using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bam.Net.Application;
using Bam.Net.Logging;
using Loggable = Bam.Net.Logging.Loggable;

namespace Bam.Net.Schema.Json
{
    public class JSchemaEnumGenerator : Loggable
    {
        [Verbosity(VerbosityLevel.Information)]
        public event EventHandler GeneratingEnums;
        
        [Verbosity(VerbosityLevel.Information)]
        public event EventHandler GeneratedEnums;

        [Verbosity(VerbosityLevel.Error)] 
        public event EventHandler GeneratingEnumsException;

        [Verbosity(VerbosityLevel.Information)]
        public event EventHandler WritingCodeFile;

        [Verbosity(VerbosityLevel.Information)]
        public event EventHandler WroteCodeFile;
        public ILogger Logger { get; set; }

        public string Workspace { get; set; }
        public void GenerateEnums(JSchemaSchemaDefinition jSchemaSchemaDefinition, string nameSpace)
        {
            try
            {
                FireEvent(GeneratingEnums, this, new JSchemaEnumGeneratorEventArgs(this){JSchemaSchemaDefinition = jSchemaSchemaDefinition});
                HashSet<JSchemaClass> jSchemaClasses = jSchemaSchemaDefinition.Classes;
                
                foreach (JSchemaClass jSchemaClass in jSchemaClasses.Where(c=> c.IsEnum))
                {
                    EnumModel model = new EnumModel(jSchemaClass, nameSpace);
                    string code = Handlebars.Render("Enum", model);
                    code.SafeWriteToFile(Path.Combine(Workspace, $"{nameSpace}.{model.Namespace}.cs"), true);
                }
                
                FireEvent(GeneratedEnums, this, new JSchemaEnumGeneratorEventArgs(this){JSchemaSchemaDefinition = jSchemaSchemaDefinition});
            }
            catch (Exception ex)
            {
                Logger.Error("Error generating enums: {0}", ex.Message);
                FireEvent(GeneratingEnumsException, this, new JSchemaEnumGeneratorEventArgs(this){Exception = ex});
            }
        }
    }
}