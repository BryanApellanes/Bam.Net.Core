# Bamdb

Bam Database (bamdb.exe) is a high level data access component which provides a client gateway to multiple data persistence layers including various RDBM systems such as Microsoft Sql, MySql, Postgres and SQLite as well as no sql solutions such as mongo and its own internal data repository system.  It is also used to generate data access objects, data transfer objects and repositories from plain CLR classes.


/generateDaoAssemblyForTypes
Generate a Dao Assembly for types in a specified namespace of a specified assembly.

/generateDaoCodeForTypes
Generate Dao source code for types in a specified namespace of a specified assembly.

/generateDtosForDaos
Generate Dto source for types in a specified namespace of a specified assembly, optionally compiling and keeping the source.

/generateSchemaRepository
In addition to generating Daos and Dtos for types in a specified namespace of a specified assembly, will 
also generate a schema specific DaoRepository for all the types found.
