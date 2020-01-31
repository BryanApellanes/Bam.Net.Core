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

function expand_tildes(){    
    export BAMTOOLKITBIN="${BAMTOOLKITBIN/#\~/$HOME}"
    export BAMTOOLKITSYMLINKS="${BAMTOOLKITSYMLINKS/#\~/$HOME}"
    echo "BAMTOOLKITBIN = ${BAMTOOLKITBIN}"
    echo "BAMTOOLKITSYMLINKS = ${BAMTOOLKITSYMLINKS}"
}

function add_to_path(){
    if [[ ! -z "$1" ]]; then
        printf "adding ${1} to the PATH\r\n\r\n"
        [[ ":$PATH:" != *":${1}:"* ]] && export PATH="${1}:${PATH}"
    fi
    printf "PATH = ${PATH}\r\n\r\n"
}

function add_symlinks_to_path(){
    if [[ -z ${BAMTOOLKITSYMLINKS} ]]; then
        printf "BAMTOOLKITSYMLINKS is not set"
        return 1
    fi
    printf "adding BAMTOOLKITSYMLINKS to the PATH\r\n\r\n"
    HOME=~

    [[ ":$PATH:" != *":${HOME}/.bam/toolkit:"* ]] && export PATH="${HOME}/.bam/toolkit:${PATH}"

    if [[ ! -z ${BAMTOOLKITSYMLINKS} ]]; then
        printf "adding ${BAMTOOLKITSYMLINKS} to the PATH\r\n"
        [[ ":$PATH:" != *":${BAMTOOLKITSYMLINKS}:"* ]] && export PATH="${BAMTOOLKITSYMLINKS}:${PATH}"
    fi
    printf "PATH = ${PATH}\r\n\r\n"
}