using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bam.Net.CoreServices.AssemblyManagement;
using Bam.Net.Logging;
using CsQuery.ExtensionMethods;

namespace Bam.Net.Automation.Scripting
{
    public class CSharpScriptContext : Loggable
    {
        public CSharpScriptContext()
        {
            Model = new CSharpScriptContextModel();
            ReferenceAssemblyResolver = new LocalReferenceAssemblyResolver();
        }

        public CSharpScriptContext(IReferenceAssemblyResolver referenceAssemblyResolver): this()
        {
            ReferenceAssemblyResolver = referenceAssemblyResolver;
        }
        
        protected IReferenceAssemblyResolver ReferenceAssemblyResolver { get;  }
        
        // TODO: memcache the resulting assembly keyed by the source code hash.  Explore assembly management service to determine fitness for incorporation there.
        /// <summary>
        /// Valid csharp code that will replace the body of the ExecuteScript method.
        /// Compiled to an extender of CSharpScriptContext.
        /// </summary>
        public string Script { get; set; }

        public virtual void ExecuteScript()
        {
            Warn("Base ExecuteScript method was called, specify 'Script' text to execute custom script code");
        }
        
        public CSharpScriptContextModel Model { get; set; }

        public CSharpScriptContext AddInclude(string variableName, object instance)
        {
            Model.AddInclude(variableName, instance);
            return this;
        }

        public CSharpScriptContext Compile(IEnumerable<Type> executionTypes, IEnumerable<Type> referenceTypes)
        {
            List<string> referencePaths = executionTypes.Select(t => t.Assembly.GetFilePath()).ToList();
            referenceTypes.ForEach(t => referencePaths.Add(ReferenceAssemblyResolver.ResolveReferenceAssemblyPath(t)));
            return Compile(referencePaths.ToArray());
        }
        
        public CSharpScriptContext Compile(params string[] referenceAssemblyPaths)
        {
            HandlebarsTemplateRenderer renderer = new HandlebarsTemplateRenderer();
            Model.Script = Script;
            string code = renderer.Render("CSharpScriptContext", Model);
            RoslynCompiler compiler = new RoslynCompiler(ReferenceAssemblyResolver);
            foreach (string referencePath in referenceAssemblyPaths)
            {
                compiler.AddAssemblyReference(referencePath);
            }
            Assembly assembly = Assembly.Load(compiler.Compile(code.Sha1(), code));
            Type type = assembly.GetTypes().FirstOrDefault(t => t.Name.Equals($"{Model.Name}ScriptContext"));
            return type.Construct<CSharpScriptContext>();
        }

        public void Execute(Type[] executionTypes, Type[] referenceTypes)
        {
            Compile(executionTypes, referenceTypes).ExecuteScript();
        }
        
        public void Execute(params Type[] referenceTypes)
        {
            Execute(referenceTypes.Select(t => ReferenceAssemblyResolver.ResolveReferenceAssemblyPath(t)).ToArray());
        }
        
        public void Execute(params string[] referenceAssemblyPaths)
        {
            Compile(referenceAssemblyPaths).ExecuteScript();
        }
    }
}