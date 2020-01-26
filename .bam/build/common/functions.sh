#!/bin/bash

function initialize_defaults() {
    if [[ -d ./env ]]; then
        cd ./env
        source defaults.sh
        cd ..
    fi
}

function build_bake(){
    if [[ -z ${BAKE} ]]; then
        rm -fr ~/.bam/tmp/bake
        dotnet publish ${BAMSRCROOT}/_tools/bake/bake.csproj -c Release -r ${RUNTIME} -o ~/.bam/tmp/bake

        export BAKE=~/.bam/tmp/bake/bake
    fi
}
