#!/bin/bash

source ../common/init.sh

ensure_bake

$BAKE /discover:${BAMSRCROOT}/_tools/ /output:${OUTPUTBIN} /outputRecipe:${OUTPUTRECIPES}${RUNTIME}-bamtoolkit.json
