#!/bin/bash

export CONTEXT=$1
export COMMAND=$2

if [[ -z ${CONTEXT} ]]; then
    export CONTEXT="build"
fi

if [[ -z $COMMAND ]]; then
    export COMMAND="build"
fi

source ./env.sh

if [[ !(-d ../${CONTEXT}) ]]; then
    printf "\r\n"
    printf "`pwd`/../${CONTEXT} folder doesn't exist.\r\n"
    printf "The command '${COMMAND}' cannot be run for the specified context '${CONTEXT}'.\r\n"
    if [[ -f contexts.sh ]]; then
        ./contexts.sh
    fi
    printf "\r\n"
    exit 1
fi

printf "executing => ${CONTEXT}/${COMMAND}.sh $3 $4 $5 $6\r\n"
cd ../${CONTEXT}
./${COMMAND}.sh $3 $4 $5 $6
cd ..