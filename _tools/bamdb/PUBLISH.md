# Bamdb.exe

Bam Database (Bamdb.exe) is a high level data access component which provides a client gateway to multiple data persistence layers including various RDBM systems such as Microsoft Sql, MySql, Postgres and SQLite as well as no sql solutions such as mongo and its own internal data repository system.  It is also used to generate data access objects, data transfer objects and repositories from plain CLR classes.

## Windows
dotnet publish -c Release -r win10-x64 [-o <outputpath>]

## Ubuntu
dotnet publish -c Release -r ubuntu.16.10-x64 [-o <outputpath>]

## Mac
dotnet publish -c Release -r osx-x64 [-o <outputpath>]