#!/bin/bash

pushd .bam/build > /dev/null
./test $1 $2
popd > /dev/null