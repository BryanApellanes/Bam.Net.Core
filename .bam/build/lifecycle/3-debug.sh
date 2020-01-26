#!/bin/bash

if [[ $1 = "-help" ]] || [[ $1 = "-?" ]] || [[ $1 = "-h" ]]; then
    printf "usage: debug <context>\r\n"
    printf "\r\n"
    printf "Call debug for the specified context; the default is 'docker'.\r\n"
    printf "Possible context values are found in the ./ci directory.\r\n"
    printf "\r\n"
    printf "docker usage: debug docker <container | image> <identifier>\r\n"
    printf "\r\n"
    printf "debug the specified docker image or container.\r\n"
    printf "\r\n"
    exit 0
fi

CONTEXT=$1

if [[ -z $1 ]]; then
    CONTEXT="docker"
fi

COMMAND="debug"
CONTAINERORIMAGE=$2
TARGETIDENTIFIER=$3

if [[ $CONTEXT = "docker" ]]; then
    if [[ -z ${CONTAINERORIMAGE} ]]; then
        CONTAINERORIMAGE="image"
    fi

    if [[ !(${CONTAINERORIMAGE} = "container") && !(${CONTAINERORIMAGE} = "image")]]; then
        printf "Invalid type specified, must be either 'container' or 'image'\r\n"
        exit 1
    fi

    if [[ -z $TARGETIDENTIFIER ]]; then
        printf "\r\n"
        printf "Please specify an identifier for the ${CONTAINERORIMAGE} to debug.\r\n"
        if [[ $CONTAINERORIMAGE = "container" ]]; then
            printf "Use 'docker ps' to list running containers\r\n"
        fi
        if [[ $CONTAINERORIMAGE = "image" ]]; then
            printf "Use 'docker images' to list available images\r\n"
        fi
        printf "\r\nexample: debug ${CONTAINERORIMAGE} 555efb\r\n"
        exit 1
    fi
fi

cd ./ci
./exec.sh ${CONTEXT} ${COMMAND} ${CONTAINERORIMAGE} ${TARGETIDENTIFIER}
cd ..
