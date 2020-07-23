using System.Text;
using Bam.Net.Bake;

namespace Bam.Net.Bake
{
    partial class Program
    {
        public static void AddArguments()
        {
            AddSwitches(typeof(ConsoleActions));
            AddValidArgument("pause", true, addAcronym: false, description: "pause before exiting, only valid if command line switches are specified.");
            AddValidArgument("output", false, true, "Specify the directory to build to.");
            AddValidArgument("outputRecipe", false, false, "On /discover, Specify the name or path of the recipe to write, default is 'recipe.json'.");
            AddValidArgument("recipePerProject", true, false, "On /discover, Specifies that all discovered projects have their own recipe written.");
            AddValidArgument("buildConfig", false, false, "On /discover or /recipe, optionally specify a build config overriding the one defined in the recipe.");

            AddValidArgument("zipRecipe", false, false, "On /zip, Specify the recipe whose 'OutputDirectory' setting is zipped.");
            
            AddValidArgument("nugetOutput", "On /nuget, Specify the directory where nuget packages are output.");
            AddValidArgument("packageVersion", false, false, "On /nuget, Specify the package version to set.");
            
            AddValidArgument("nugetSource", false, false, "On /nugetPush | /nugetRestore, Specify the source to push to; default is 'nuget.org'");
            AddValidArgument("nugetApiKey", false, false, "On /nugetPush, Specify the apiKey used to push to the nuget repository");
            
            AddValidArgument("versionRecipe", true, false, "On /version, specify the recipe to update versions for");
            AddValidArgument("major", true, false, "On /version, Increment the major version number");
            AddValidArgument("minor", true, false, "On /version, Increment the minor version number");
            AddValidArgument("patch", true, false, "On /version, Increment the patch version number");
            AddValidArgument("dev", true, false, "On /version, Include the commit as build number");
            AddValidArgument("test", true, false, "On /version, Include '-test' as the build number");
            AddValidArgument("staging", true, false, "On /version, Include '-rc+{Commit} as the build");
            AddValidArgument("release", true, false, "On /version, Remove build and prerelease.");
            AddValidArgument("gitRepo", false, false, "On /version, Specify the repository to get the commit hash from");
            AddValidArgument("reset", true, false, "On /version, reset the version to the one specified by the /version option");
        }
    }
}