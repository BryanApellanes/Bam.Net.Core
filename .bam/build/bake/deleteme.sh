#!/bin/bash

source ../common/functions.sh

hello "from the main script"

printf "BAMLIFECYCLE = ${BAMLIFECYCLE}\r\n"

initialize

printf "after initialize BAMLIFECYCLE = ${BAMLIFECYCLE}\r\n"
