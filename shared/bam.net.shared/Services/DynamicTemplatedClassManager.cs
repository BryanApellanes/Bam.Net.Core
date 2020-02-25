using System;
using System.Linq;
using System.Reflection;

namespace Bam.Net.Services
{
    public class DynamicTemplatedClassManager
    {
        public DynamicTemplatedClassManager()
        {
            HashAlgorithm = HashAlgorithms.SHA1;
            TemplateRenderer = new HandlebarsTemplateRenderer();
            Compiler = new RoslynCompiler();
        }
        
        public ITemplateRenderer TemplateRenderer { get; set; }
        public ICompiler Compiler { get; set; }
        
        public HashAlgorithms HashAlgorithm { get; set; }
        
        public T Compile<T>(string templateName, object model) where T: class
        {
            // TODO: cache this
            string shouldBeCode = TemplateRenderer.Render(templateName, model);
            byte[] assemblyBytes = Compiler.Compile(shouldBeCode.HashHexString(HashAlgorithm), shouldBeCode);
            Assembly assembly = Assembly.Load(assemblyBytes);
            Type type = assembly.GetTypes().FirstOrDefault(t => t.ExtendsType<T>());
            return type.Construct() as T;
        }
    }
}