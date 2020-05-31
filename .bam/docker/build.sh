#!/bin/bash

if [[ -z "$1" ]] || [[ $1 = "-help" ]] || [[ $1 = "-?" ]] || [[ $1 = "-h" ]]; then
    printf "usage: build.sh <image-tag> [Dockerfile]\r\n"
    printf "\r\n"
    printf "Creates an image using the specified tag/name and specified dockerfile.\r\n"
    printf "If no docker file is specified the file {image-tag}.Dockerfile is used if\r\n"
    printf "it exists otherwise 'Dockerfile' is used.\r\n"
    printf "\r\n"
    exit 0
fi

DOCKERBUILDNUM=$(<buildnum)
echo $(($DOCKERBUILDNUM + 1)) > ./buildnum

TAG=$1
FILE=Dockerfile

if [[ -f ${TAG}.Dockerfile ]]; then
    FILE=${TAG}.Dockerfile
fi

if [[ $# -eq 2 ]]; then
    FILE=$2
fi

printf "Calling docker build => 'docker build -t ${TAG} . -f ${FILE}'\r\n"
docker build -t ${TAG} . -f ${FILE}
