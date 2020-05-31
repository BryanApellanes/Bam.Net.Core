#!/bin/bash

cd defaults
for FILE in ./* 
do
    CURRENTVARIABLE=`echo ${FILE} | sed 's#./##'`
    export $CURRENTVARIABLE=$(<./${FILE})    
done

cd ..

source ./runtime.sh