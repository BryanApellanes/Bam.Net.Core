# Bam.exe

Bam.exe is a tool used to manage BamFramework features and functionality.  In addition to bam apps, it works with asp.net core razor page applications.

TL;DR

- bam /config
- bam /init
- bam /import
- bam /gen:all
- bam /addModel:[modelName],[type1:propertyName1],[type2:propertyName2]...
- bam /addPage:[path/to/newPage]
- bam /webpack
- bam /build
- bam /push
- bam /deploy

## /config
Writes the default config file backing up the current file if it exists.

## /init
Clones the bam.js repository into wwwroot/bam.js, writes Startup.cs and sample AppModule classes into the AppModules folder. Startup.cs is backed up if the file already exists; AppModules are only written if they do not already exist.

## /import
Reads data files (csv, json, yaml) from the folders of the same name found in AppData.  Dynamic types are derived from the data.

## /gen
Data access code.  The code generated represents the dynamic types derived from the import process.  

### /gen:src
Generates C# source code for all the dynamic types derived from the import process.  Source code files are placed in a folder named "_gen" inside the AppData folder.

### /gen:bin
Generates binaries for all the dynamic types derived from the import process.

### /gen:models
Renders source code for all AppModel definitions found in AppModels/Definitions/*.yaml.

### /gen:dbjs
Generates dao C# source code for any *.db.js file found in the current directory or subdirectories.  This command delegates to [laotze.exe](../laotze).

### /gen:repo
Generates repositories for types found in any namespace whose name ends with "AppModels".  This is accomplished by first building the csproj file using "dotnet publish" then reflecting over the resulting dll.  This command delegates to [troo.exe](../troo).

### /gen:all
Calls all generation operations in the order, src -> dbjs -> bin -> repo.

## /addPage:[path/to/newPage]
Adds the files that constitute a new page.  Files added are:

- Pages/[path/to/newPage].cshtml
- Pages/[path/to/newPage].cshtml.cs
- wwwroot/bam.js/pages/[path/to/newPage].js
- wwwroot/bam.js/configs/[path/to/newPage]/webpack.config.js

## /addModel:[modelName],[type1:propertyName1],[type2:propertyName2]...
Adds a model definition to AppModels/Definitions and re-generates all models from the current set of definitions.  Existing model source files are overwritten.

## /webpack
Uses WebPack to pack each bam.js page found in wwwroot/bam.js/pages using corresponding configs found in wwwroot/bam.js/configs.

## /build
Creates a Dockerfile then creates a docker image.

## /push
Tag the docker image and push it to the bamapps docker registry, for example: 

```
docker tag {{app-name}} bamapps/images:{{app-name}}
docker push bamapps/images:{{app-name}}
```

## /clean
Clears all dynamic types and namespaces from the dynamic type manager.