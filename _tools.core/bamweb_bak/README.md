# Bam Web Server

#TL;DR

```
bamweb /S /content:c:/bam/content [/verbose]
```

The Bam Web Server hosts an HTTP server that serves content 
from the directory specified in the config file by the key "ContentRoot".

- Hosts a Bam specific HTTP server
- Will kill the bamd (BamDaemon) process if it is running