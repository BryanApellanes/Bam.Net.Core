#!/bin/bash

source ../common/init.sh

ensure_bake

${BAKE} /discover:${BAMSRCROOT}/_lib/ /output:${OUTPUTLIB} /outputRecipe:${OUTPUTRECIPES}${RUNTIME}-bamfx-lib.json