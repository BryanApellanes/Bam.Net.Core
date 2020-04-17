#!/bin/bash

BLACK='\033[0;30m'
DARKRED='\033[0;31m'
DARKGREEN='\033[0;32m'
DARKYELLOW='\033[0;33m'
DARKBLUE='\033[0;34m'
DARKPURPLE='\033[0;35m'
DARKCYAN='\033[0;36m'
GRAY='\033[0;37m'
DARKGRAY='\033[1;30m'
RED='\033[1;31m'
GREEN='\033[1;32m'
YELLOW='\033[1;33m'
BLUE='\033[1;34m'
PURPLE='\033[1;35m'
CYAN='\033[1;36m'
WHITE='\033[1;37m'
NOCOLOR='\033[0m'
NC='\033[0m'

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

function ensure_bamtest(){
    if [[ -z ${BAMTEST} || !(-f ${BAMTEST}) ]]; then
        build_bamtest
    fi
}

function build_bamtest(){
    rm -fr ~/.bam/tmp/build_bamtest
    dotnet publish ${BAMSRCROOT}/_tools/bamtest/bamtest.csproj -c Release -r ${RUNTIME} -o ~/.bam/tmp/bamtest
    
    export BAMTEST=~/.bam/tmp/bamtest/bamtest
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
        printf "BAMTOOLKITSYMLINKS is not set\r\n"
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

function set_git_commit(){
    if [[ -z ${GITCOMMIT} ]]; then
        printf "Setting GITCOMMIT\r\n"
        export GITCOMMIT=`git rev-parse --short HEAD`
    fi
    printf "GITCOMMIT = ${GITCOMMIT}\r\n"
}