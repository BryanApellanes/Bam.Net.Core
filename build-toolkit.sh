#!/bin/bash

cd .bam/build

./configure tools
./clean tools
./build tools

cd ../../