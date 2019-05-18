# bake.exe

bake.exe is used to build and test the BamFramework and toolkit.  The compilation process is delegated to dotnet.exe.

## TL;DR
To discover csproj files in all subfolders of the folder specified and record them in recipe.json:

```
bake /discover:.\path_to__tools.core\
```

To build discovered csproj files:

```
bake /toolkit:.\recipe.json
```

To discover and build in one command:

```
bake /all:.\path_to_tools.core\
```

## Command Line Options

These are the available command line options bake.exe recognizes.  All options are assumed to be in the format /name:value or just /name if no value is required.

- output    Specify the directory to build to
- recipe    Specify the recipe file to use

# Recipe File

The recipe file describes what project files to build by providing an array of csproj file paths.