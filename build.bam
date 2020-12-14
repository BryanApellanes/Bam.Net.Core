#!/bin/bash

function configureCleanBuild(){
    CONTEXT=$1
    if [[ -z ${CONTEXT} ]]; then
        CONTEXT=tests
    fi
    if [[ -f "configure" ]]; then
        ./configure ${CONTEXT}
    else
        echo "`pwd`: 'configure' script not found"
    fi
    if [[ -f "clean" ]]; then
        ./clean ${CONTEXT}
    else
        echo "`pwd`: 'clean' script not found"
    fi    
    if [[ -f "build" ]]; then
        ./build ${CONTEXT}
    else
        echo "`pwd`: 'build' script not found"
    fi
}

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
        pushd .bam/build
        ./build
        popd
    else
        configureCleanBuild lib
        configureCleanBuild tools
        configureCleanBuild tests
    fi
}

build