#!/bin/bash

function initialize_defaults() {
    if [[ -d ./env ]]; then
        cd ./env
        source defaults.sh
        cd ..
    fi
}

function ensure_bake(){
    if [[ -z ${BAKE} || !(-f ${BAKE}) ]]; then
        build_bake    
    fi
}

function build_bake(){
    rm -fr ~/.bam/tmp/bake
    dotnet publish ${BAMSRCROOT}/_tools/bake/bake.csproj -c Release -r ${RUNTIME} -o ~/.bam/tmp/bake

    export BAKE=~/.bam/tmp/bake/bake
}
