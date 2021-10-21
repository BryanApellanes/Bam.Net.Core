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
        source ./init.sh $1
        popd > /dev/null
        pushd .bam/build > /dev/null

        print_line "GITHUB_SHA = ${GITHUB_SHA}" ${GREEN}

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
        echo "./.bam/build not found, add, then fetch the 'build' submodule"
        echo "git submodule add https://github.com/BryanApellanes/build.git ./bam/build"
    fi
}

BAMSRCROOT=$1
if [[ -z ${BAMSRCROOT} ]]; then
    if [[ "${OSTYPE}" == "cygwin" || "${OSTYPE}" == "msys" ]]; then
        BAMSRCROOT=`pwd -W`
    else
        BAMSRCROOT=`pwd`
    fi      
fi

build ${BAMSRCROOT}