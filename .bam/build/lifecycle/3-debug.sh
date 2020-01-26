#!/bin/bash

if [[ $1 = "-help" ]] || [[ $1 = "-?" ]] || [[ $1 = "-h" ]]; then
    printf "usage: debug <context>\r\n"
    printf "\r\n"
    exit 0
fi

CONTEXT=$1

if [[ -z $1 ]]; then
    CONTEXT="tools"
fi


cd ./common
./exec.sh ${CONTEXT} $COMMAND
cd ..
