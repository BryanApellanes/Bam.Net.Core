#!/bin/bash

CURDIR=`pwd`
cd ..
source ../common/init.sh
echo baloney
ensure_bake

$BAKE /discover:${BAMSRCROOT}/_tests/core/ /output:${OUTPUTBIN} /outputRecipe:${OUTPUTRECIPES}${RUNTIME}-bamtoolkit-tests.json

cd ${CURDIR}