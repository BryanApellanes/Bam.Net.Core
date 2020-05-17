#!/bin/bash

if [[ $1 = "-help" ]] || [[ $1 = "-?" ]] || [[ $1 = "-h" ]]; then
    printf "usage: test <implementation>\r\n"
    printf "\r\n"
    printf "Call test for the specified context; the default is 'tools'.\r\n"
    printf "\r\n"
    exit 0
fi


export CONTEXT="tools"
export COMMAND="test"

cd ./common
./exec.sh ${CONTEXT} ${COMMAND} $1 $2 $3 $4
cd ..