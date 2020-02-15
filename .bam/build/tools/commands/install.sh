#!/bin/bash

source ../common/init.sh

if [[ ! -f ${DIST}/${RUNTIME}-bamtoolkit.zip  ]]; then 
    printf "${DIST}/${RUNTIME}-bamtoolkit.zip not found, run build-toolkit.sh first.\r\n"
    exit 1
else
    rm -fr ${BAMTOOLKITBIN}
    mkdir -p ${BAMTOOLKITBIN}
    unzip ${DIST}/${RUNTIME}-bamtoolkit.zip -d ${BAMTOOLKITBIN}
    source ./commands/set-path.sh ${BAMTOOLKITSYMLINKS}
fi

export BAMTOOLKITBIN
printf "Installed BamToolkit to ${BAMTOOLKITBIN}\r\n\r\n"