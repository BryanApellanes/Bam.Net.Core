# Bam.exe

Bam.exe is a tool used to manage BamFramework features and functionality within a net core application.

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
The /config switch writes the default config file backing up the current file if it exists.

## /init
The /init switch clones the bam.js repository into wwwroot/bam.js, writes Startup.cs and sample AppModule classes into the AppModules folder. Startup.cs is backed up if the file already exists; AppModules are only written if they do not already exist.

## /import
The /import switch reads data files (csv, json, yaml) from the folders of the same name found in AppData.  Dynamic types are derived from the data.

## /gen
The /gen switch generates data access code.  The code generated represents the dynamic types derived from the import process.  

### /gen:src
Specifying the /gen:src argument generates C# source code for all the dynamic types derived from the import process.  Source code files are placed in a folder named "_gen" inside the AppData folder.

### /gen:bin
Specifying the /gen:bin argument generates binaries for all the dynamic types derived from the import process.

### /gen:models
Specifying the /gen:models argument renders source code for all AppModel definitions found in AppModels/Definitions/*.yaml.

### /gen:dbjs
Specifying the /gen:dbjs argument generates dao C# source code for any *.db.js file found in the current directory or subdirectories.  This command delegates to [laotze.exe](../laotze).

### /gen:repo
Specifying the /gen:repo argument generates repositories for types found in any namespace whose name ends with "AppModels".  This is accomplished by first building the csproj file using "dotnet publish" then reflecting over the resulting dll.  This command delegates to [troo.exe](../troo).

### /gen:all
Specifying the /gen:all argument calls all generation operations in the order, src -> dbjs -> bin -> repo.

## /addPage:[path/to/newPage]
The /addPage:[path/to/newPage] switch adds the files that constitute a new page.  Files added are:

- Pages/[path/to/newPage].cshtml
- Pages/[path/to/newPage].cshtml.cs
- wwwroot/bam.js/pages/[path/to/newPage].js
- wwwroot/bam.js/configs/[path/to/newPage]/webpack.config.js

## /addModel:[modelName],[type1:propertyName1],[type2:propertyName2]...
The /addModel switch adds a model definition to AppModels/Definitions and re-generates all models from the current set of definitions.  Existing model source files are overwritten.

## /webpack
Use WebPack to pack each bam.js page found in wwwroot/bam.js/pages using corresponding configs found in wwwroot/bam.js/configs.

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