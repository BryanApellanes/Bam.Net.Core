# Bamdb

Bam Database (bamdb) is a high level data access component which provides a client gateway to multiple data persistence layers including various RDBM systems such as Microsoft Sql, MySql, Postgres and SQLite as well as no sql solutions such as mongo and its own internal data repository system.  It is also used to generate data access objects, data transfer objects and repositories from plain CLR classes.

## Code Generation

The following describes the options for code generation using bamdb.

### /generateDaoAssemblyForTypes
Generate a Dao Assembly for types in a specified namespace of a specified assembly.

### /generateDaoCodeForTypes
Generate Dao source code for types in a specified namespace of a specified assembly.

### /generateDtosForDaos
Generate Dto source for types in a specified namespace of a specified assembly, optionally compiling and keeping the source.

### /generateSchemaRepository
In addition to generating Daos and Dtos for types in a specified namespace of a specified assembly, will 
also generate a schema specific DaoRepository for all the types found.

Accepts a `config` command line argument specifying the path to a yaml serialized GenerationConfig:

```
bamdb /config:<pathToConfig>
```

An example of a GenerationConfig.yaml file:

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


## Database Server

The following describes the options available to serve data using bamdb.

 