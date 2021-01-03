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
        pushd .bam/build/common > /dev/null
        source ./init.sh
        popd > /dev/null
        echo `curdir` > "./.bam/build/overrides/BAMSRCROOT"
        export BAMOVERRIDES=`curdir`/.bam/build/overrides
        export_bam_overrides # first time sets $BAMSRCROOT
        set_git_commit

        pushd .bam/build > /dev/null

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

        set_git_commit

        popd > /dev/null
    else
        echo "./.bam/build not found add and fetch the build submodule"
    fi
}

build