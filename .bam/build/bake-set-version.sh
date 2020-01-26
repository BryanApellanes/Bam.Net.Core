#!/bin/bash

if [[ $1 = "-help" ]] || [[ $1 = "-?" ]] || [[ $1 = "-h" ]]; then
    printf "usage: bake-set-version.sh\r\n"
    printf "\r\n"
    printf "First builds the 'bake' utility.\r\n"
    printf "Then uses 'bake' to set the version of the bam toolkit as specified in the recipe\r\n"
    printf "./recipes/$RUNTIME-bamtoolkit.json.\r\n"
    printf "\r\n"
    exit 0
fi

export BAMSRCROOT=../../

source ./get-os-runtime.sh
source ./build-bake.sh

$BAKE /version:Patch /test /versionRecipe:./recipes/$RUNTIME-bamfx-lib.json
$BAKE /version:Patch /test /versionRecipe:./recipes/$RUNTIME-bamtoolkit.json
