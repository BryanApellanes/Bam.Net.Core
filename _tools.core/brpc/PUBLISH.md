# Brpc.exe

Using Brpc you can create and expose a web service by creating a class
and serving that class with brpc.exe.

## Windows
dotnet publish -c Release -r win10-x64 [-o <outputpath>]

## Ubuntu
dotnet publish -c Release -r ubuntu.16.10-x64 [-o <outputpath>]

## Mac
dotnet publish -c Release -r osx-x64 [-o <outputpath>]