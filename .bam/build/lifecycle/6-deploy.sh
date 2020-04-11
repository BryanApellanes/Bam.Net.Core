#!/bin/bash


if [[ $1 = "-help" ]] || [[ $1 = "-?" ]] || [[ $1 = "-h" ]]; then
    printf "usage: deploy <context> <test | staging | release> [ image-tag ]\r\n"
    printf "\r\n"
    printf "Call deploy for the specified context; the default is 'tools'.\r\n"
    printf "\r\n"
    exit 0
fi

CONTEXT=$1

if [[ -z $1 ]]; then
    export CONTEXT="tools"
fi

export COMMAND="deploy"

cd ./common
./exec.sh ${CONTEXT} ${COMMAND} 
cd ..