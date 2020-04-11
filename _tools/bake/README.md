# Bake

`bake` is used to build and test the BamFramework and toolkit.  The compilation process is delegated to `dotnet`.

## TL;DR
To discover csproj files in all subfolders of a specified folder and record them in recipe.json:

```
bake /discover:.\path_to__Bam.Net.Core_slash_tools\
```

To build discovered csproj files:

```
bake /recipe:.\recipe.json
```

To discover and build in one command:

```
bake /all:.\path_to_tools.core\
```

## Command Line Options

`bake` recognizes the following command line options.  All options are assumed to be in the format /name:value or just /name if a value is not required.

- output - Specify the directory to build to
- recipe - Specify the recipe file to use

# Recipe File

The recipe file describes what project files to build by providing an array of csproj file paths.

