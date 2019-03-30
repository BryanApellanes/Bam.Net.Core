using System;
using System.Linq;
using System.Reflection;
using Bam.Net.Logging;

namespace Bam.Net.Automation.Scripting
{
    public class CSharpScriptContext : Loggable
    {
        string toCompile = @"using System;
using Bam.Net.Automation.Scripting;
namespace Bam.Net.Dynamic
{
    public class {{Name}}ScriptContext : CSharpScriptContext
    {
        public {{Name}}ScriptContext : base() {}

{{#each IncludeInstances}}
        public {{TypeName}} {{ScriptVariable}} { get; set; }
{{/each}}

        public override void ExecuteScript() 
        {
{{Script}}
        }
    }
}";

        public CSharpScriptContext()
        {
            Model = new CSharpScriptContextModel();
        }
        
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
/*

using System;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Bam.Net;
using Bam.Net.CommandLine;

namespace Bam.Shell.Jobs
{
    public class WorkerPropertyCompiler
    {
        [ConsoleAction]
        public void Test()
        {
            
            string toCompile = @"using System;
namespace Bam.Net.Dynamic
{
    public class WorkerPropertyResolver
    {
        public WorkState WorkState { get; set; }

        public Job Job { get; set; }

        public {{PropertyType}} Resolve()
        {
            return Resolve({{PropertyResolverLambda}});
        }

        public {{PropertyType}} Resolve(Func<WorkState, Job> propertyResolver)
        {
            return propertyResolver(WorkState, Job);
        }
    }
}";
            RoslynCompiler compiler = new RoslynCompiler();
            compiler.AddAssemblyReference(
                "C:\\bam\\nuget\\global\\runtime.win-x64.microsoft.netcore.app\\2.2.2\\runtimes\\win-x64\\lib\\netcoreapp2.2\\Microsoft.CSharp.dll");
            compiler.AddAssemblyReference(typeof(ExpandoObject).Assembly.Location);
            compiler.AddAssemblyReference("C:\\bam\\nuget\\global\\runtime.win-x64.microsoft.netcore.app\\2.2.2\\runtimes\\win-x64\\lib\\netcoreapp2.2\\System.Runtime.dll");
            compiler.AddAssemblyReference("C:\\bam\\nuget\\global\\runtime.win-x64.microsoft.netcore.app\\2.2.2\\runtimes\\win-x64\\lib\\netcoreapp2.2\\System.Core.dll");
            byte[] assBytes = compiler.Compile(toCompile.Sha1(), toCompile);
            Assembly compiled = Assembly.Load(assBytes);
            Type dynamicContext = compiled.GetTypes().FirstOrDefault();
            object instance = dynamicContext.Construct();
            Expect.IsNotNull(dynamicContext);
            object output = instance.Invoke("Fun", (Func<string>)(()=> "baloney"));
            Expect.IsNotNull(output);
            Console.WriteLine(output.ToString());
        }
    }
}*/