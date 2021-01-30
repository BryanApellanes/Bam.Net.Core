rem see https://www.sjcnet.co.uk/2016/07/30/net-core-code-coverage-using-opencover/ for updated information

bamtest.exe /TestsWithCoverage /type:Unit /testReportHost:int.bamapps.net /testReportPort:80 /tag:test-tag-123 /search:.\%1