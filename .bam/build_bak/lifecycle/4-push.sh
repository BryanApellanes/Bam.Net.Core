#!/bin/bash

if [[ $1 = "-help" ]] || [[ $1 = "-?" ]] || [[ $1 = "-h" ]]; then
    printf "usage: push <context> [additional arguments]\r\n"
    printf "\r\n"
    printf "Call push for the specified context; the default is 'tests'.\r\n"
    printf "\r\n"
    exit 0
fi

CONTEXT=$1

if [[ -z $1 ]]; then
    export CONTEXT="tests"
fi

export COMMAND="push"

cd ./common
./exec.sh ${CONTEXT} ${COMMAND}
cd ..
