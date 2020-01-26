#!/bin/bash
# The first argument should be the local filesystem path to the root of the Bam.Net git repository

if [[ $1 = "-help" ]] || [[ $1 = "-?" ]] || [[ $1 = "-h" ]]; then
    printf "usage: bake-nuget.sh\r\n"
    printf "\r\n"
    printf "First builds the 'bake' utility found at the specified root directory.\r\n"
    printf "Then uses 'bake' to nuget pack the bam toolkit as specified in the recipe\r\n"
    printf "./recipes/$RUNTIME-bamtoolkit.json.\r\n"
    printf "\r\n"
    exit 0
fi

source ./set-src-root.sh
source ./get-os-runtime.sh

echo building bake from $BAMSRCROOT
./build-bake.sh $BAMSRCROOT

echo "baking nuget package(s) for `pwd`/recipes/$RUNTIME-bamfx-lib.json"
~/.bam/bake/bake /nuget:./recipes/$RUNTIME-bamfx-lib.json
