#!/bin/bash

if [[ -z "${BAMLIFECYCLE}" ]]; then
    export BAMLIFECYCLE=$(<./defaults/BAMLIFECYCLE)
    echo "BAMLIFECYCLE = ${BAMLIFECYCLE}"
fi

if [[ -z "${BAMSRCROOT}" ]]; then
    export BAMSRCROOT=$(<./defaults/BAMSRCROOT)
    echo "BAMSRCROOT = ${BAMSRCROOT}"
fi

if [[ -z "${OUTPUTRECIPES}" ]]; then
    export OUTPUTRECIPES=$(<./defaults/OUTPUTRECIPES)
    echo "OUTPUTRECIPES = ${OUTPUTRECIPES}"
fi

source ./runtime.sh