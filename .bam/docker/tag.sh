#!/bin/bash

if [[ -z "$1" ]] || [[ $1 = "-help" ]] || [[ $1 = "-?" ]] || [[ $1 = "-h" ]]; then
    printf "usage: tag.sh <image-name> [remote-registry-path]\r\n"
    printf "\r\n"
    printf "Tag the specified 'image-name' to be pushed to the specified remote registry or docker hub if no remote registry is specified.\r\n"
    printf "\r\n"
    exit 0
fi

IMAGE=$1
REMOTEREGISTRY=docker.io/bamapps/$IMAGE

if [[ $# -eq 2 ]]; then
    REMOTEREGISTRY=$2
fi

docker tag $IMAGE $REMOTEREGISTRY