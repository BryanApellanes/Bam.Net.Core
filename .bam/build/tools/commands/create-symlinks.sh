#!/bin/bash

if [[ -z $BAMTOOLKITSYMLINKS ]]; then
    BAMTOOLKITSYMLINKS=$1 # where to put symlinks
fi
echo "BAMTOOLKITSYMLINKS = ${BAMTOOLKITSYMLINKS}"

if [[ -z $BAMTOOLKITBIN ]]; then
    BAMTOOLKITBIN=$2
fi
echo "BAMTOOLKITBIN = ${BAMTOOLKITBIN}"

if [[ -z $BAMTOOLKITBIN ]]; then
    printf "\r\n"
    printf "Please set the environment variable BAMTOOLKITBIN or specify the toolkit bin path as the second parameter to this script.\r\n"
    printf "\r\n"
    exit 1
fi

printf "BAMTOOKITSYMLINKS will go ${BAMTOOLKITSYMLINKS}\r\n"
curdir=`pwd`
cd ${BAMTOOLKITBIN}
for TOOLNAME in $(ls -d */ | sed 's#/##')
do
    printf "Setting permission mode to 755 for ${BAMTOOLKITBIN}/${TOOLNAME}/${TOOLNAME}\r\n"
    chmod 755 ${BAMTOOLKITBIN}/${TOOLNAME}/${TOOLNAME}    
    if [[ -L ${BAMTOOLKITSYMLINKS}/${TOOLNAME} ]]; then
        printf "Removing existing link ${BAMTOOLKITSYMLINKS}/${TOOLNAME}"
        rm ${BAMTOOLKITSYMLINKS}/${TOOLNAME}
    fi
    printf "Adding symlink ${BAMTOOLKITSYMLINKS}/${TOOLNAME} => ${BAMTOOLKITBIN}/${TOOLNAME}/${TOOLNAME}\r\n"  
    echo "ln -s ${BAMTOOLKITBIN}/${TOOLNAME}/${TOOLNAME} ${BAMTOOLKITSYMLINKS}/${TOOLNAME}"
    ln -s ${BAMTOOLKITBIN}/${TOOLNAME}/${TOOLNAME} ${BAMTOOLKITSYMLINKS}/${TOOLNAME}    
    printf "\r\n"
done
cd $curdir

export BAMTOOLKITSYMLINKS
printf "\r\n"
printf "BamToolkit sym links added to ${BAMTOOLKITSYMLINKS}\r\n\r\n"