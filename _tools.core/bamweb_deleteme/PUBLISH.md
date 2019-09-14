# Bamweb.exe

The Bam Web Server hosts an HTTP server that serves content 
from the directory specified in the config file by the key "ContentRoot".

## Windows
dotnet publish -c Release -r win10-x64 [-o <outputpath>]

## Ubuntu
dotnet publish -c Release -r ubuntu.16.10-x64 [-o <outputpath>]

## Mac
dotnet publish -c Release -r osx-x64 [-o <outputpath>]