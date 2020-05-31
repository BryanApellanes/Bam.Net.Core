#!/bin/bash


if [[ -z "$1" ]] || [[ $1 = "-help" ]] || [[ $1 = "-?" ]] || [[ $1 = "-h" ]]; then
    printf "usage: push.sh <image-name> [remote-registry-path]\r\n"
    printf "\r\n"
    printf "Tag the specified 'image-name' then push it to the specified remote registry or docker hub if no remote registry is specified.\r\n"
    printf "\r\n"
    exit 0
fi

IMAGENAME=$1
VERSION=$(<../semver/version)
IMAGE=${IMAGENAME}:${VERSION}
REMOTEREGISTRY=docker.io/bamapps

if [[ $# -eq 2 ]]; then
    if [[ $2 = "latest" ]]; then
        IMAGE=${IMAGENAME}:latest
    else
        IMAGE=${IMAGENAME}:$2
    fi
fi

./login.sh
REMOTEIMAGE=${REMOTEREGISTRY}/${IMAGE}

printf "Tagging docker image => 'docker tag ${IMAGENAME} ${REMOTEIMAGE}'\r\n"
docker tag ${IMAGENAME} ${REMOTEIMAGE}
printf "Pushing docker image => 'docker push ${REMOTEIMAGE}'\r\n"
docker push ${REMOTEIMAGE}