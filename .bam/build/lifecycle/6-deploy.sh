#!/bin/bash


if [[ $1 = "-help" ]] || [[ $1 = "-?" ]] || [[ $1 = "-h" ]]; then
    printf "usage: deploy <context> <test | staging | release> [ image-tag ]\r\n"
    printf "\r\n"
    printf "Call deploy for the specified context; the default is 'docker'.\r\n"
    printf "Possible context values are found in the `pwd`/ci directory.\r\n"
    printf "\r\n"
    printf "docker usage: deploy docker <environment> [image-tag]\r\n"
    printf "\r\n"
    printf "where environment is one of the following:\r\n"
    printf " - test\r\n"
    printf " - staging\r\n"
    printf " - release\r\n"
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