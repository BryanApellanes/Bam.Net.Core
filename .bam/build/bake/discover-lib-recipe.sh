#!/bin/bash

# The first argument should be the local filesystem path to the root of the Bam.Net git repository

if [[ $1 = "-help" ]] || [[ $1 = "-?" ]] || [[ $1 = "-h" ]]
then
    printf "usage: bake-discover-lib-recipe {{path to Bam.Net.Core}}\r\n"
    printf "\r\n"
    printf "Builds a temporary copy of 'bake' and uses it to discover tools in the specified Bam.Net.Core root.\r\n"
    printf "'bake' looks for *.csproj files in the child directories of the _tools directory in the specified root.\r\n"
    printf "\r\n"
    exit 0
fi

source ../common/init.sh

build_bake

${BAKE} /discover:${BAMSRCROOT}/_lib/ /output:/tmp/bam/fx /outputRecipe:$OUTPUTRECIPES$RUNTIME-bamfx-lib.json
