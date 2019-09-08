using System;
using Bam.Net.Bake;
using Bam.Net.Testing;
using Bam.Net.Testing.Unit;

namespace Bam.Net.Automation.Tests
{
    public class RecipeTests : CommandLineTestInterface
    {
        [UnitTest]
        //[TestGroup("Recipe")]
        public void CanLoadProject()
        {
            Project project = "./bake.csproj.xml".FromXmlFile<Project>();
        }
        
        [UnitTest]
        [TestGroup("Recipe")]
        public void CanChangeProjectReferenceToPackageReference()
        {
            string modifyingProject = "./bake.csproj.xml";
            Recipe recipe = Recipe.FromProject(modifyingProject);
            recipe.ReferencesProject(modifyingProject, "bam.net.core").IsTrue();
            
            recipe.SetPackageReference("bam.net.core");
            
            recipe.ReferencesProject(modifyingProject, "bam.net.core").IsFalse();
            recipe.ReferencesPackage(modifyingProject, "bam.net.core").IsTrue();
        } 
    }
}