#!/bin/bash

if [[ $1 = "-help" ]] || [[ $1 = "-?" ]] || [[ $1 = "-h" ]]; then
    printf "usage: build-toolkit.sh <version>\r\n"
    printf "\r\n"
    printf "Build the bam toolkit found at the root source directory specified in the file _srcroot or the default of `pwd`/Bam.Net.Core.\r\n"
    printf "\r\n"
    printf "The <version> may be the name of a branch or a commit hash; <version> should not be confused with the semver version\r\n"
    printf "that may be assigned at release."
    printf "\r\n"

    exit 0
fi

export BAMSRCROOT=../../

source ./get-os-runtime.sh 

./bake-discover-tools-recipe.sh
./bake-tools-recipe.sh 
source ./bake-zip-toolkit.sh