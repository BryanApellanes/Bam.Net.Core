#!/bin/bash

source ../common/init.sh

ensure_bake

printf "deleting ${OUTPUTBIN}\r\n"
rm -fr ${OUTPUTBIN}

${BAKE} /recipe:./recipes/${RUNTIME}-bamtoolkit-tests.json