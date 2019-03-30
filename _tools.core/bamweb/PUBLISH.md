# Bdb.exe

Bam Database (bdb.exe) is a high level data access component which provides a client gateway to multiple data persistence layers including various rdbms systems such as Microsoft Sql, MySql, Postgres and SQLite as well as no sql solutions such as mongo and its own internal data repository system.

## Windows
dotnet publish -c Release -r win10-x64 [-o <outputpath>]

## Ubuntu
dotnet publish -c Release -r ubuntu.16.10-x64 [-o <outputpath>]

## Mac
dotnet publish -c Release -r osx-x64 [-o <outputpath>]