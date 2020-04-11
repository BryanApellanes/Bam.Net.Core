#!/bin/bash

cd .bam/build

./configure lib
./clean lib
./build lib

cd ../../