# bamtest

A Unit and Integration test runner for running Bam.Net test definitions

## Options
- /dir - The directory to look for test assemblies in
- /search - The search pattern to use to locate test assemblies, the default is *Tests.*
- /type - The type of tests to run, either 'Unit' or 'Integration', default is 'Unit'
- 

## Unit Test Command Line Args
/dir:. /search:bamtest.exe /type:Unit /testReportHost:int.bamapps.net /testReportPort:80 /tag:testingtherunner 