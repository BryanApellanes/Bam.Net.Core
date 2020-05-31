#!/bin/bash

source ./env.sh

export DOCKER_USER=`head -1 ./creds/${IMAGEREGISTRY} | tail -1`
export DOCKER_PASSWORD=`head -2 ./creds/${IMAGEREGISTRY} | tail -1`

if [[ -z $DOCKER_USER ]]; then
    printf "\r\n"
    printf "DOCKER_USER not set; specify credentials in the file `pwd`/creds/${IMAGEREGISTRY}\r\n"
    printf "\r\n"
    exit 1
fi

if [[ -z $DOCKER_PASSWORD ]]; then
    printf "\r\n"
    printf "DOCKER_PASSWORD not set; specify credentials in the file `pwd`/creds/${IMAGEREGISTRY}\r\n"
    printf "\r\n"
    exit 1
fi

printf "logging into docker => 'docker login ${REMOTEREGISTRY} -u $DOCKER_USER --password-stdin'\r\n"
echo "$DOCKER_PASSWORD" | docker login ${REMOTEREGISTRY} -u $DOCKER_USER --password-stdin