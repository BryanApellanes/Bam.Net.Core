#!/bin/bash

source ./init.sh

./discover-tools-recipe.sh
./discover-lib-recipe.sh

./write-version-code.sh
