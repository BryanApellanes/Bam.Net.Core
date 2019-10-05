using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bam.Net;

namespace Bam.Templates.Shell.Models
{
    public class CommandLineDelegatedClassModel
    {
        public CommandLineDelegatedClassModel()
        {
            ReferenceTypes = new HashSet<Type>();
            Namespace = "Bam.Shell";
        }

        string _nameSpace;

        public string Namespace
        {
            get { return _nameSpace; }
            set
            {
                _nameSpace = value;
                SetNamespaces();
            }
        }
        public HashSet<Type> ReferenceTypes { get; set; }
        public ConcreteClassModel ConcreteClass { get; set; }
        public ProviderBaseClassModel ProviderBaseClass { get; set; }
        public DelegatorClassModel DelegatorClass { get; set; }

        public void AddReferenceType(Type type)
        {
            ReferenceTypes.Add(type);
            SetNamespaces();
        }
        
        public static CommandLineDelegatedClassModel Create(string concreteTypeName,
            string baseTypeName,
            params string[] methodNames)
        {
            return new CommandLineDelegatedClassModel()
            {
                ConcreteClass = new ConcreteClassModel()
                {
                    BaseTypeName = baseTypeName, 
                    ConcreteTypeName = concreteTypeName,
                    Methods = methodNames.Select(m => new ConcreteMethodModel {ConcreteMethodName = m}).ToArray()
                },
                ProviderBaseClass = new ProviderBaseClassModel()
                {
                    BaseTypeName = baseTypeName,
                    Methods = methodNames.Select(m => new ProviderBaseClassMethodModel {MethodName = m}).ToArray()
                },
                DelegatorClass = new DelegatorClassModel()
                {
                    BaseTypeName = baseTypeName,
                    Methods = methodNames.Select(m => new DelegatorMethodModel {MethodName = m}).ToArray()
                }
            };
        }

        public void SetBaseTypeName(string baseTypeName)
        {
            ConcreteClass.BaseTypeName = baseTypeName;
            ProviderBaseClass.BaseTypeName = baseTypeName;
            DelegatorClass.BaseTypeName = baseTypeName;
            foreach (DelegatorMethodModel method in DelegatorClass.Methods)
            {
                method.BaseTypeName = baseTypeName;
            }
        }
        
        public void SetMethods(params string[] methodNames)
        {
            ConcreteClass.Methods =
                methodNames.Select(m => new ConcreteMethodModel {ConcreteMethodName = m}).ToArray();
            ProviderBaseClass.Methods =
                methodNames.Select(m => new ProviderBaseClassMethodModel {MethodName = m}).ToArray();
            DelegatorClass.Methods =
                methodNames.Select(m => new DelegatorMethodModel {MethodName = m}).ToArray();
        }
        
        private void SetNamespaces()
        {
            StringBuilder usingNamespaces = new StringBuilder();
            HashSet<string> nameSpaces = new HashSet<string>();
            foreach (string ns in ReferenceTypes.Select(t=> t.Namespace))
            {
                if (!nameSpaces.Contains(ns))
                {
                    nameSpaces.Add(ns);
                    usingNamespaces.AppendLine($"using {ns};");
                }
            }

            if (ConcreteClass != null)
            {
                ConcreteClass.UsingNameSpaces = usingNamespaces.ToString();
                ConcreteClass.NameSpace = Namespace;
            }

            if (ProviderBaseClass != null)
            {
                ProviderBaseClass.NameSpace = Namespace;
            }

            if (DelegatorClass != null)
            {
                DelegatorClass.NameSpace = Namespace;
            }
        }
    }
}