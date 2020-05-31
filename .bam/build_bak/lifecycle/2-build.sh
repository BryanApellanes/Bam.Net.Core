#!/bin/bash

if [[ $1 = "-help" ]] || [[ $1 = "-?" ]] || [[ $1 = "-h" ]]; then
    printf "usage: build <context>\r\n"
    printf "\r\n"
    printf "Call build for the specified context; the default is 'tools'.\r\n"
    printf "\r\n"
    exit 0
fi

CONTEXT=$1

if [[ -z $1 ]]; then
    CONTEXT="tools"
fi

COMMAND="build"

cd ./common
./exec.sh ${CONTEXT} $COMMAND
cd ..
