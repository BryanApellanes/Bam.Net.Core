#!/bin/bash

if [[ $1 = "-help" ]] || [[ $1 = "-?" ]] || [[ $1 = "-h" ]]; then
    printf "usage: clean.sh <image-name>\r\n"
    printf "\r\n"
    printf "Using docker, deletes exited containers and dangling images."
    printf "\r\n"

else

docker rm $(docker ps -q -f 'status=exited')
docker rmi $(docker images -q -f "dangling=true")

fi