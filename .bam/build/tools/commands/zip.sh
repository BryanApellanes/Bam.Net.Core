#!/bin/bash

source ../common/init.sh

ensure_bake

${BAKE} /zip /zipRecipe:./recipes/${RUNTIME}-bamtoolkit.json /output:${OUTPUTBIN}