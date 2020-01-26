#!/bin/bash

CONTEXT=$1

if [[ -z $1 ]]; then
    CONTEXT="bake"
fi

COMMAND="configure"

cd ./common
./exec.sh ${CONTEXT} $COMMAND
cd ..
