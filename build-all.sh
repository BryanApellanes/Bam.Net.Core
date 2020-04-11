#!/bin/bash

cd .bam/build
./configure lib
./clean lib
./build lib

./configure tools
./clean tools
./build tools

./configure tests
./clean tests
./build tests