#!/bin/bash


# gsr - generate schema repository
# ta - type assembly - the assembly to scan for types
# sn - schemaName - the name to give the generated schema
# fns - from name space - the namespace in the assembly to scan for types
# cfi - check for ids - if affirmative ('yes', '1', 'true'), then check each type for an "Id" property and fail if it is missing
# uis - use inheritance schema - if affirmative ('yes', '1', 'true'), then the resulting repository extends "DatabaseRepository" instead of DaoRepository
# ws - write source - the path to write generated source files to
bamdb /gsr /ta:.\bam.net.core.dll /sn:AuthSettings /fns:Bam.Net.CoreServices.Auth.Data /cfi:yes /uis:no /ws:~/.bam/src/generated/Bam.Net.CoreServices/Auth/Data/Generated_Dao