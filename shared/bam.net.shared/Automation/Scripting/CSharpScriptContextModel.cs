using System.Collections.Generic;
using Bam.Net.Testing;

namespace Bam.Net.Automation.Scripting
{
    public class CSharpScriptContextModel
    {
        List<IncludeDescriptor> _includeDescriptors;
        public CSharpScriptContextModel()
        {
            Name = "Custom";
            Script = "System.Console.WriteLine(\"Custom script output\");";
            _includeDescriptors = new List<IncludeDescriptor>();
        }
        
        public string Name { get; set; }

        public IncludeDescriptor[] IncludeInstances
        {
            get { return _includeDescriptors.ToArray(); }
            set{_includeDescriptors = new List<IncludeDescriptor>(value);}
        }

        public string Script { get; set; }

        public void AddInclude(string variableName, object instance)
        {
            _includeDescriptors.Add(new IncludeDescriptor()
            {
                TypeName = instance.GetType().Name,
                ScriptVariable = variableName,
                Instance =  instance
            });
        }
    }
}