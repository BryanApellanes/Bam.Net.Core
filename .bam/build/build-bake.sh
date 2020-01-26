#!/bin/bash

if [[ $1 = "-help" ]] || [[ $1 = "-?" ]] || [[ $1 = "-h" ]]; then
    printf "usage: build-bake.sh <image-name>\r\n"
    printf "\r\n"
    printf "Builds 'bake' from the source found in `pwd`/Bam.Net.Core/_tools/bake/."
    printf "\r\n"
    exit 0
fi

source ./get-os-runtime.sh

dotnet publish ${BAMSRCROOT}/_tools/bake/bake.csproj -c Release -r ${RUNTIME} -o ~/.bam/bake