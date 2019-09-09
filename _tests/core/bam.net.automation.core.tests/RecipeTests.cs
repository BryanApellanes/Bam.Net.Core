using System;
using System.IO;
using Bam.Net.Bake;
using Bam.Net.Testing;
using Bam.Net.Testing.Unit;

namespace Bam.Net.Automation.Tests
{
    public class RecipeTests : CommandLineTestInterface
    {
        [UnitTest]
        [TestGroup("Recipe")]
        public void CanLoadProject()
        {
            Project project = "./bake.csproj.xml".FromXmlFile<Project>();
        }
        
        [UnitTest]
        [TestGroup("Recipe")]
        public void CanChangeProjectReferenceToPackageReference()
        {
            string testProjectFileName = $"./test_{6.RandomLetters()}_project-package.csproj";
            File.Copy("./test_project-package.csproj.xml", testProjectFileName);
            Recipe recipe = Recipe.FromProject(testProjectFileName);
            recipe.ReferencesAsProject(testProjectFileName, "bam.net.core").IsTrue();
            
            recipe.ReferenceAsPackage("bam.net.core");
            
            recipe.ReferencesAsProject(testProjectFileName, "bam.net.core").IsFalse();
            recipe.ReferencesPackage(testProjectFileName, "bam.net.core").IsTrue();
            
            File.Delete(testProjectFileName);
        }

        [UnitTest]
        [TestGroup("Recipe")]
        public void CanChangePackageReferenceToProjectReference()
        {
            string testProjectFileName = $"./test_{4.RandomLetters()}_package-project.csproj";
            File.Copy("./test_package-project.csproj.xml", testProjectFileName);
            Recipe recipe = Recipe.FromProject(testProjectFileName);
            recipe.ReferencesPackage(testProjectFileName, "bam.net.core").IsTrue();
            
            recipe.ReferenceAsProject("bam.net.core", "../some/path/");
            
            recipe.ReferencesPackage(testProjectFileName, "bam.net.core").IsFalse();
            recipe.ReferencesAsProject(testProjectFileName, "../some/path/bam.net.core.csproj");
            
            File.Delete(testProjectFileName);
        }
    }
}