#!/bin/bash

function clean(){
    if [[ -d "./.bam/build" ]]; then
        pushd .bam/build/common > /dev/null
        source ./init.sh $1
        popd > /dev/null
        pushd .bam/build > /dev/null

        print_line "GITHUB_SHA = ${GITHUB_SHA}" ${GREEN}

        clean_artifacts

        ./clean lib
        ./clean tools
        ./clean tests

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

clean ${BAMSRCROOT}