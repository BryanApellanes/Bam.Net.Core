using System.Text;

namespace Bam.Net.Bake
{
    partial class Program
    {
        public static void AddArguments()
        {
            AddSwitches(typeof(ConsoleActions));
            AddValidArgument("pause", true, addAcronym: false, description: "pause before exiting, only valid if command line switches are specified.");
            AddValidArgument("output", false, true, "Specify the directory to build to.");
            AddValidArgument("outputRecipe", false, false, "On /discover, Specify the name of the recipe to write, default is 'recipe.json'.");
            AddValidArgument("recipePerProject", true, false, "On /discover, Specifies that all discovered projects have their own recipe written.");
            AddValidArgument("zipRecipe", false, false, "On /zip, Specify the recipe whose 'OutputDirectory' setting is zipped.");
            AddValidArgument("nugetOutput", "On /nuget, Specify the directory where nuget packages are output.");
            AddValidArgument("packageVersion", false, false, "On /nuget, Specify the package version to set.");
            AddValidArgument("name", false, false, "On /nuget, Specify the name of the package source to add");
        }
    }
}