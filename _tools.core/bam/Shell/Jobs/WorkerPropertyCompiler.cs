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
}