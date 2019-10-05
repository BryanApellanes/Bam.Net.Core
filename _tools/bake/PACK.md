# bake.exe /nuget

bake.exe /nuget is used to package the BamFramework and toolkit into nuget packages.

## Pack Recipe
With bake:

`bake.exe /nuget:<path-to-recipe.json> [/outputNuget:<path-to-output-nuget-packages-to>]`

With dotnet:

`dotnet pack [project-file] -c [Debug|Release] -o [output-directory] [-p:PackageVersion=packageVersion]` 
