#!/bin/bash

source ../common/init.sh

ensure_bake

$BAKE /discover:${BAMSRCROOT}/_tests/core/ /output:${OUTPUTBIN} /outputRecipe:${OUTPUTRECIPES}${RUNTIME}-bamtoolkit-tests.json
