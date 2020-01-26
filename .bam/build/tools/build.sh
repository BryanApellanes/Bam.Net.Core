#!/bin/bash

source ../common/init.sh

ensure_bake

printf "deleting ${OUTPUTBIN}"
rm -fr ${OUTPUTBIN}

${BAKE} /recipe:./recipes/${RUNTIME}-bamtoolkit.json

printf "zipping toolkit to ${OUTPUT}\r\n"

${BAKE} /zip /zipRecipe:./recipes/${RUNTIME}-bamtoolkit.json /output:${OUTPUTBIN}

./commands/install.sh