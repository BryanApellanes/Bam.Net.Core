#!/bin/bash

cd .bam/build
./configure lib
./clean lib
./build lib

./configure tools
./clean tools
./build tools