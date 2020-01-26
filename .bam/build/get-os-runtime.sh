#!/bin/bash

if [[ "$OSTYPE" == "linux-gnu" ]]
    then RUNTIME=ubuntu.16.10-x64
fi
if [[ "$OSTYPE" == "darwin"* ]]
    then RUNTIME=osx-x64
fi
if [[ "$OSTYPE" == "cygwin" ]]
    then RUNTIME=win10-x64
fi
if [[ "$OSTYPE" == "msys" ]]
    then RUNTIME=win10-x64
fi
if [[ "$OSTYPE" == "freebsd"* ]]
    then RUNTIME=osx-x64
fi

export RUNTIME
echo 'RUNTIME is' ${RUNTIME}
