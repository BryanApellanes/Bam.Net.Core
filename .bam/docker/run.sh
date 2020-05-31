#!/bin/bash


if [[ -z "$1" ]] || [[ $1 = "-help" ]] || [[ $1 = "-?" ]] || [[ $1 = "-h" ]]; then
    printf "usage: run.sh <image-name> <image-id> <innerport>:<outerport>\r\n"
    printf "\r\n"
    printf "Starts a bash prompt in the specified container to explore and debug its content.\r\n"
    printf "\r\n"
    exit 0
fi

SUFFIX=`date | md5`

NAME=$1
IMAGE=$2
PORTS=$3

if [[ -z "$1" ]]
    then
        printf "Please specify a name for the container.\r\n\r\n"
        exit 1
fi 

if [[ -z "$2" ]]
    then
        printf "Please specify the image id to run.\r\n\r\n"
        exit 1
fi

if [[ -z "$3" ]]
    then
        PORTS=8080:8080
else
    PORTS=$3
fi

docker run -d -p $PORTS --name $NAME-$SUFFIX $IMAGE