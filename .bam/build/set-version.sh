#!/bin/bash

if [[ !(-z "$1") ]]; then
    export BAMLIFECYCLE=$1
fi

./bake-discover-tools-recipe.sh
./bake-discover-lib-recipe.sh

./bake-set-version.sh
