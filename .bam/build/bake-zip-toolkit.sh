#!/bin/bash

if [[ $1 = "-help" ]] || [[ $1 = "-?" ]] || [[ $1 = "-h" ]]; then
    printf "usage: bake-zip-toolkit.sh\r\n"
    printf "\r\n"
    printf "Uses 'bake' to zip the bam toolkit as specified in the recipe\r\n"
    printf "./recipes/$RUNTIME-bamtoolkit.json.\r\n"
    printf "\r\n"
    exit 0
fi

export BAMSRCROOT=../../.

source ./get-os-runtime.sh 

./build-bake.sh

mkdir -p /tmp/bam/bin
OUTPUT=/tmp/bam/bin
printf "OUTPUT = $OUTPUT\r\n"
~/.bam/bake/bake /zip /zipRecipe:./recipes/${RUNTIME}-bamtoolkit.json /output:${OUTPUT}
