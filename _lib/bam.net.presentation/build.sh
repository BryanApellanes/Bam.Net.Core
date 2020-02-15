#!/bin/bash

CONFIG=$1
OUTPUT=$2
dotnet publish -c $${CONFIG} -o ${OUTPUT}