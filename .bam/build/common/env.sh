#!/bin/bash

if [[ -z ${CONTEXT} ]]; then
    export CONTEXT="build"
fi

if [[ -d ${CONTEXT} ]]; then
    cd ${CONTEXT}
    if [[ -f ./env/defaults.sh ]]; then
        source ./env/defaults.sh
    fi
    cd ..
fi