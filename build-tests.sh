#!/bin/bash

cd .bam/build
./configure tests
./clean tests
./build tests