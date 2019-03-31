using System;
using System.Linq;
using System.Reflection;
using Bam.Net.Logging;

namespace Bam.Net.Automation.Scripting
{
    public class CSharpScriptContext : Loggable
    {
        public CSharpScriptContext()
        {
            Model = new CSharpScriptContextModel();
        }
        
        // TODO: memcache the resulting assembly keyed by the source code.  Explore assembly service to determine fitness for incorporation there.
        /// <summary>
        /// Valid csharp code that will replace the body of the ExecuteScript method.
        /// Compiled to an extender of CSharpScriptContext.
        /// </summary>
        public string Script { get; set; }

        public virtual void ExecuteScript()
        {
            Warn("Base ExecutScript method was called, specify 'Script' text to execute custom script code");
        }
        
        public CSharpScriptContextModel Model { get; set; }

        public CSharpScriptContext AddInclude(string variableName, object instance)
        {
            Model.AddInclude(variableName, instance);
            return this;
        }

        public CSharpScriptContext Compile(params string[] referenceAssemblyPaths)
        {
            HandlebarsTemplateRenderer renderer = new HandlebarsTemplateRenderer();
            Model.Script = Script;
            string code = renderer.Render("CSharpScriptContext", Model);
            RoslynCompiler compiler = new RoslynCompiler();
            foreach (string referencePath in referenceAssemblyPaths)
            {
                compiler.AddAssemblyReference(referencePath);
            }
            Assembly assembly = Assembly.Load(compiler.Compile(code.Sha1(), code));
            Type type = assembly.GetTypes().FirstOrDefault(t => t.Name.Equals($"{Model.Name}ScriptContext"));
            return type.Construct<CSharpScriptContext>();
        }
        
        public void Execute(params string[] referenceAssemblyPaths)
        {
            Compile(referenceAssemblyPaths).ExecuteScript();
        }
    }
}