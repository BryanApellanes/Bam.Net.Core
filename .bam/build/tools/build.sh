#!/bin/bash

source ../common/init.sh

ensure_bake

${BAKE} /recipe:./recipes/${RUNTIME}-bamtoolkit.json

printf "zipping toolkit to ${OUTPUT}\r\n"

${BAKE} /zip /zipRecipe:./recipes/${RUNTIME}-bamtoolkit.json /output:${OUTPUT}

./commands/install.sh