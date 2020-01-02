using System;
using System.Linq;
using Bam.Net.Testing.Unit;

namespace Bam.Net.Bake.Tests
{
    [Serializable]
    public class ProjectPackageTests
    {
        [UnitTest]
        public void CanReferencePackageProperties()
        {
           // figure out how to update nuget package version in csproj file
           /*Project p;
           ProjectPropertyGroup propertyGroup = p.PropertyGroup.FirstOrDefault(pg => pg.PackageId != null);
           if (propertyGroup != null)
           {
               propertyGroup.Version
           }*/
        }
    }
}