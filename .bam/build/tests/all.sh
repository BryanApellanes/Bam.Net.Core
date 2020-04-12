#!/bin/bash


./configure.sh
./clean.sh
./build.sh

cd ./run
./run.sh $1 $2 $3
cd ..