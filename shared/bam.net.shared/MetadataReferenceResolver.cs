using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bam.Net.Services.DataReplication;
using Microsoft.CodeAnalysis;

namespace Bam.Net
{
    public class MetadataReferenceResolver
    {
        public MetadataReferenceResolver(params Type[] types)
        {
            this.Types = types;
        }
        
        public Type[] Types { get; set; }

        public IEnumerable<MetadataReference> GetMetaDataReferences()
        {
            return GetMetaDataReferences(Types);
        }

        public IEnumerable<MetadataReference> GetMetaDataReferences(params Type[] types)
        {
            HashSet<Assembly> assemblies = types.Select(t => t.Assembly).ToHashSet(); 
            foreach (Assembly ass in assemblies)
            {
                if (ass != null)
                {
                    yield return MetadataReference.CreateFromFile(ass.Location);
                }
            }
        }
    }
}