#!/bin/bash

export CONTEXT=$1
export COMMAND=$2

cd ./common
source ./init.sh
cd ..

cd ${CONTEXT}
if [[ !(-f ./commands/${COMMAND}.sh) ]]; then
    printf "Specified command not found ${CONTEXT}/${COMMAND}\r\n"
    exit 1
fi

source ./commands/${COMMAND}.sh $3 $4 $5 $6

cd ..

