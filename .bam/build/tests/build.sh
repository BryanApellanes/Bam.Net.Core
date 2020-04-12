#!/bin/bash

source ../common/init.sh

ensure_bake

printf "deleting ${TESTBIN}\r\n"
rm -fr ${TESTBIN}

${BAKE} /recipe:./recipes/${RUNTIME}-bamtoolkit-tests.json

./commands/zip.sh