#!/bin/bash

function configure(){
    if [[ -d "./.bam/build" ]]; then
        pushd .bam/build/common > /dev/null
        source ./init.sh $1
        popd > /dev/null
        pushd .bam/build > /dev/null

        print_line "GITHUB_SHA = ${GITHUB_SHA}" ${GREEN}

        ./configure lib
        ./configure tools
        ./configure tests

        popd > /dev/null
    else
        echo "./.bam/build not found, add, then fetch the 'build' submodule"
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

configure ${BAMSRCROOT}