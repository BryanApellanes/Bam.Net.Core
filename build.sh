#!/bin/bash

function foreachSubmodule(){
    COMMAND=$1
    SUBMODULES=($(git submodule | awk '{print $2}'))
    for SUBMODULE in "${SUBMODULES[@]}"; do
        pushd $SUBMODULE > /dev/null
        echo `pwd`
        $1
        popd > /dev/null
    done
}

function build(){
    if [[ -d "./.bam/build" ]]; then
        pushd .bam/build > /dev/null
        pushd ./common > /dev/null
        source ./init.sh
        popd > /dev/null
        clean_artifacts

        ./configure lib
        ./clean lib
        ./build lib
        ./configure tools
        ./clean tools
        ./build tools
        ./configure tests
        ./clean tests
        ./build tests
        popd > /dev/null
    else
        echo "./.bam/build not found add and fetch the build submodule"
    fi
}

build