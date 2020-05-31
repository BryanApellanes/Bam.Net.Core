#!/bin/bash

if [[ -z "$1" || $1 = "-help" ]] || [[ $1 = "-?" ]] || [[ $1 = "-h" ]]; then
    printf "usage: generate-key.sh <email>\r\n"
    printf "\r\n"
    printf "Using ssh-keygen, generates a new RSA key pair placing them into ./_ssh\r\n"
    printf "\r\n"
    printf "\r\n"

    exit 0
else

EMAIL=$1

ssh-keygen -t rsa -b 4096 -C "$EMAIL" -f ./_ssh/id_rsa -q -N ""

fi