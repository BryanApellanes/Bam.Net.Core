#!/bin/bash

if [[ $1 = "-help" ]] || [[ $1 = "-?" ]] || [[ $1 = "-h" ]]; then
    printf "usage: test <implementation>\r\n"
    printf "\r\n"
    printf "Call test for the specified context; the default is 'docker'.\r\n"
    printf "Possible implementation values are found in the `pwd`/ci directory.\r\n"
    printf "\r\n"
    exit 0
fi

CONTEXT=$1

if [[ -z $1 ]]; then
    export CONTEXT="docker"
fi

export COMMAND="test"

cd ./ci
./exec.sh ${CONTEXT} ${COMMAND} $2 $3 $4 $5
cd ..