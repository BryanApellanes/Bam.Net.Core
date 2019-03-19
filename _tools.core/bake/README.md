# bake.exe

bake.exe is used to build and test the BamFramework and toolkit.

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


