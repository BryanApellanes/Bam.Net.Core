# Bamdb

Bam Database (bamdb) is a high level data access component which provides a client gateway to multiple data persistence layers including various RDBM systems such as Microsoft Sql, MySql, Postgres and SQLite as well as no sql solutions such as mongo and its own internal data repository system.  It is also used to generate data access objects, data transfer objects and repositories from plain CLR classes.

## Arg Zero Code Generation
The following describes arg zero based code generation options using bamdb. See also, [Bam Shell and ArgZero](../bam/Shell).

- `gen Schema` - Generate a bamdb schema file by analyzing a database specified in a yaml serialized [DaoConfig](./Shell/CodeGen/DaoConfig.cs).  See [BamDb Schema File](#BamDbSchemaFile).
    - `/output` - The directory path to output generated files to; default value is `"./_gen"`.
    - `/config` - The file containing serialized DaoConfigs; default value is `"./.bamdb.daoconfigs"`.
    - `/configName` - The name of the config to use.  If there is only one entry specified in the config file, it is used.  If there is more than one entry in the config file and the `/configName` argument is omitted, you are prompted for the config to use.    
- `gen Dao` - Generate Dao classes for a specified schema file.
    - `/output` - The directory path to output generated code to; default value is `"./_gen/src"`.
    - `/namespace` - The namespace to place generated classes into.
    - `/schema` - The path to the schema definition file.
- `gen GraphQL` - Generate an assembly containing classes for graphQL traversal of persistable types.

## Command Line Switch Code Generation
The following describes command line switch based code generation options using bamdb.

#### /generateDaoAssemblyForTypes
Generate a Dao Assembly for types in a specified namespace of a specified assembly.

#### /generateDaoCodeForTypes
Generate Dao source code for types in a specified namespace of a specified assembly.

#### /generateDtosForDaos
Generate Dto source for types in a specified namespace of a specified assembly, optionally compiling and keeping the source.

#### /generateSchemaRepository
In addition to generating Daos and Dtos for types in a specified namespace of a specified assembly, will 
also generate a schema specific DaoRepository for all the types found.

Accepts a `config` command line argument specifying the path to a yaml serialized GenerationConfig:

```
bamdb /config:<pathToConfig>
```

An example of a DaoRepoGenerationConfig.yaml file:

```yaml
SchemaName: MyApplicationSchema
WriteSourceTo: /tmp/MyApplicationSchema
TypeAssembly: /tmp/bin/myapplication.dll
FromNameSpace: My.Application.Data
ToNameSpace: My.Appliction.Data.Dao
```

Alternatively, each option can be specified on the command line, note that command line options are case sensitive:

```
bamdb /schemaName:MyApplicationSchema /writeSource:C:\tmp\MyApplicationSchema /typeAssembly:C:\bin\myapplication.dll /fromNameSpace:My.Application.Data /toNameSpace:My.Application.Data.Dao
```

## BamDb Schema File
A bamdb schema file is a json formatted file containing meta data that describes a database table structure and relationships.

## Database Server

The following describes the options available to serve data using bamdb.

TODO: finish this

 