#!/bin/bash

if [[ $1 = "-help" ]] || [[ $1 = "-?" ]] || [[ $1 = "-h" ]]; then
    printf "usage: configure <context>\r\n"
    printf "\r\n"
    printf "Call configure for the specified context; the default is 'docker'.\r\n"
    printf "Possible context values are found in the ./ci directory.\r\n"
    printf "\r\n"
    exit 0
fi

CONTEXT=$1

if [[ -z $1 ]]; then
    CONTEXT="docker"
fi

COMMAND="configure"

cd ./common
./exec.sh ${CONTEXT} $COMMAND
cd ..
