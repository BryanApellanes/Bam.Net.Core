#!/bin/bash

source ../common/init.sh

ensure_bake

$BAKE /discover:${BAMSRCROOT}/_tools/ /output:${OUTPUT} /outputRecipe:${OUTPUTRECIPES}${RUNTIME}-bamtoolkit.json
