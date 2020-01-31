#!/bin/bash

source ../common/init.sh

ensure_bake

echo "BAMLIFECYLE = ${BAMLIFECYCLE}"

./commands/discover.sh

$BAKE /version:Patch /${BAMLIFECYCLE} /versionRecipe:./recipes/${RUNTIME}-bamtoolkit-tests.json
