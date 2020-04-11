#!/bin/bash

source ../common/init.sh

ensure_bake

printf "deleting ${OUTPUTBIN}/tests\r\n"
rm -fr ${OUTPUTBIN}/tests

${BAKE} /recipe:./recipes/${RUNTIME}-bamtoolkit-tests.json
